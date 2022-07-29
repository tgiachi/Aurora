using Aurora.Api.Entities.Impl.Services;
using Aurora.Api.JsonConverters;
using Aurora.Api.MethodEx;
using Aurora.Turbine.Api.Converters;
using Aurora.Turbine.Api.Data;
using Aurora.Turbine.Api.Interfaces;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ConfigurationSubstitution;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using ILogger = Serilog.ILogger;
using IsoDateTimeConverter = Aurora.Turbine.Api.Converters.IsoDateTimeConverter;

namespace Aurora.Turbine.Api.Services
{
    public class TurbineWebEngine : ITurbineWebEngine
    {
        private WebApplicationBuilder _webApplicationBuilder = null!;
        private LoggerConfiguration _loggerConfiguration = new();
        private ILogger _logger = null!;
        private WebApplication _webApplication = null!;
        private TurbineConfig _turbineConfig = null!;
        private string _forceMode = null!;
        private readonly List<Type> _dbContextsAutoMigrate = new();

        public event ITurbineWebEngine.TurbineReadyDelegate? OnTurbineReady;
        public event ITurbineWebEngine.TurbineAppBuiltDelegate? OnTurbineAppBuilt;


        public Task InitLogger(LoggerConfiguration loggerConfiguration)
        {
            _loggerConfiguration = loggerConfiguration;

            return Task.CompletedTask;
        }

        public Task<WebApplicationBuilder> Build(TurbineConfig config, params string[] args)
        {
            _turbineConfig = config;
            _webApplicationBuilder = WebApplication.CreateBuilder(new WebApplicationOptions()
            {

                Args = args
            });

            if (_webApplicationBuilder.Environment.IsDevelopment())
            {
                _loggerConfiguration = _loggerConfiguration.MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                    .WriteTo.Console();
            }
            else
            {
                _loggerConfiguration = _loggerConfiguration.MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                    .WriteTo.Console(new JsonFormatter(renderMessage: true));
            }

            Log.Logger = _loggerConfiguration.CreateLogger();
            _logger = Log.Logger;


            _logger.Debug("Adding Env vars");
            _webApplicationBuilder.Configuration
                .AddEnvironmentVariables()
                .EnableSubstitutions();

            _logger.Debug("Using AutoFac");
            _webApplicationBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());


            return Task.FromResult(_webApplicationBuilder);
        }

        public void ConfigureServices(Func<ContainerBuilder, ConfigurationManager, ContainerBuilder> containerBuilder)
        {
            _webApplicationBuilder.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                _logger.Debug("Configuring isendu services");
                builder = containerBuilder.Invoke(builder, _webApplicationBuilder.Configuration);

                builder
                    .ScanModules()
                    .AddTaskQueueService()
                    .AddEventBus();

                if (_webApplicationBuilder.Environment.IsDevelopment())
                {
                    if (!string.IsNullOrEmpty(_forceMode))
                    {
                        builder.ForceSetEnv(_forceMode);
                    }
                    else
                    {
                        builder.ForceSetEnv("alpha");
                    }

                }
            });
        }

        public void RegisterDbContextForAutoMigrate<TDbContext>() where TDbContext : DbContext
        {
            _dbContextsAutoMigrate.Add(typeof(TDbContext));
        }

        public void ForceEnvironmentMode(string mode)
        {
            _forceMode = mode;
        }



        public async Task Run()
        {
            InitDefaultSettings();

            if (_turbineConfig.UseSwagger)
            {
                _webApplicationBuilder.Services.AddEndpointsApiExplorer();
                _webApplicationBuilder.Services.AddSwaggerGen();
            }

            _webApplicationBuilder.Services.AddRouting(options => options.LowercaseUrls = true);

            _webApplication = _webApplicationBuilder.Build();
            OnTurbineAppBuilt?.Invoke(_webApplication);

            _logger.Information("Starting application");

            if (_turbineConfig.UseSwagger)
            {
                _logger.Information("Starting Swagger");

                _webApplication.UseSwagger();
                _webApplication.UseSwaggerUI();
            }

            if (_turbineConfig.IsMapControllers)
            {
                _logger.Information("Mapping controllers");
                _webApplication.UseAuthentication();
                _webApplication.MapControllers();
            }

            OnTurbineReady?.Invoke(_webApplication.Services);

            await AutoMigrateDbContexts();
            await _webApplication.RunAsync();
        }

        public void AddContextFactory<TDbContext>(Action<DbContextOptionsBuilder> options, bool addToAutoMigrate) where TDbContext : DbContext
        {
            _logger.Information("Adding DbContext {DbContext}", typeof(DbContext));
            _webApplicationBuilder.Services.AddDbContextFactory<TDbContext>(options);

            if (addToAutoMigrate)
                RegisterDbContextForAutoMigrate<TDbContext>();

        }

        private async Task AutoMigrateDbContexts()
        {
            if (_dbContextsAutoMigrate.Any())
            {
                foreach (var dbContextType in _dbContextsAutoMigrate)
                {
                    using var scope = _webApplication.Services.CreateScope();
                    _logger.Information("Db context auto migrate: {Type}", dbContextType.Name);
                    var dbContext = scope.ServiceProvider.GetService(dbContextType) as DbContext;

                    await dbContext?.Database.MigrateAsync()!;
                }

                var dbSeedService = _webApplication.Services.GetService<DbSeedService>();

                if (dbSeedService != null)
                {
                    await dbSeedService?.ExecuteDbSeeds()!;
                }
            }
        }

        private void InitDefaultSettings()
        {
            // Enum to string
            _webApplicationBuilder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonConverterForType());
                if (_turbineConfig.EnableRestDateTimeMills)
                {
                    options.JsonSerializerOptions.Converters.Add(new MillisDateTimeConverter());
                }
                else
                {
                    options.JsonSerializerOptions.Converters.Add(new IsoDateTimeConverter());
                }
            });

            _webApplicationBuilder.Services.Add(new ServiceDescriptor(typeof(TurbineConfig), _turbineConfig));

            _webApplicationBuilder.Host.UseSerilog();
        }
    }
}
