using Aurora.Api.Interfaces.Services;
using Aurora.Api.Services;
using Autofac;

namespace Aurora.Api.MethodEx
{
    public static class CsvServiceMethodEx
    {
        public static ContainerBuilder AddCsvService(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<CsvParserService>().As<ICsvParserService>().AsSelf()
                .InstancePerLifetimeScope();

            return containerBuilder;
        }
    }
}
