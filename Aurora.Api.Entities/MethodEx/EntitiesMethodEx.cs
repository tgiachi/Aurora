using System.Reflection;
using Aurora.Api.Attributes;
using Aurora.Api.Entities.Attributes;
using Aurora.Api.Entities.Impl.Services;
using Aurora.Api.Entities.Interfaces.Services;
using Aurora.Api.Utils;
using Autofac;
using AutoMapper;


namespace Aurora.Api.Entities.MethodEx
{
    public static class EntitiesMethodEx
    {
        // public static IServiceCollection AddEntitiesDataAccess(this IServiceCollection serviceCollection)
        // {
        //     var dataAccess = AssemblyUtils.GetAttribute<DataAccessAttribute>();
        //     dataAccess.ForEach(d => { serviceCollection.AddTransient(d); });
        //     return serviceCollection;
        // }

        public static ContainerBuilder AddDbSeedService(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DbSeedService>().As<IDbSeedService>().AsSelf().SingleInstance();

            return containerBuilder;
        }

        public static ContainerBuilder AddEntitiesDataAccess(this ContainerBuilder serviceCollection)
        {
            var dataAccess = AssemblyUtils.GetAttribute<DataAccessAttribute>();
            dataAccess.ForEach(d => { serviceCollection.RegisterType(d).AsSelf().InstancePerDependency(); });

            var dbSeeds = AssemblyUtils.GetAttribute<DbSeedAttribute>();
            dbSeeds.ForEach(d =>
            {
                serviceCollection.RegisterType(d).As(AssemblyUtils.GetInterfaceOfType(d)).AsSelf()
                    .InstancePerDependency();
            });

            return serviceCollection;
        }


        public static ContainerBuilder AddDtoMappers(this ContainerBuilder containerBuilder)
        {
            var mappers = AssemblyUtils.GetAttribute<DtoMapperAttribute>();
            var configuration = new MapperConfiguration(cfg =>
            {
                mappers.ForEach(dto =>
                {
                    var attribute = dto.GetCustomAttribute<DtoMapperAttribute>();
                    cfg.CreateMap(attribute?.SourceType, attribute?.TargetType).ReverseMap();
                    containerBuilder.RegisterType(dto).AsSelf().InstancePerDependency();
                });
            });
            containerBuilder.RegisterInstance(new Mapper(configuration)).As<IMapper>().AsSelf();


            return containerBuilder;
        }
    }
}
