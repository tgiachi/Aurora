using Aurora.Turbine.Api.Data;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Aurora.Turbine.Api.Interfaces
{
    public interface ITurbineWebEngine
    {
        delegate Task TurbineReadyDelegate(IServiceProvider provider);

        event TurbineReadyDelegate OnTurbineReady;

        Task InitLogger(LoggerConfiguration loggerConfiguration);

        Task<WebApplicationBuilder> Build(TurbineConfig config, params string[] args);

        void ConfigureServices(Func<ContainerBuilder, ConfigurationManager, ContainerBuilder> containerBuilder);

        void RegisterDbContextForAutoMigrate<TDbContext>() where TDbContext : DbContext;

        void ForceEnvironmentMode(string mode);
        Task Run();

        void AddContextFactory<TDbContext>(Action<DbContextOptionsBuilder> options, bool addToAutoMigrate) where TDbContext : DbContext;
    }
}
