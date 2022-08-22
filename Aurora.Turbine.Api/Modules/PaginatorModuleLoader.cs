using Aurora.Turbine.Api.Interfaces;
using Aurora.Turbine.Api.Services;
using Autofac;

namespace Aurora.Turbine.Api.Modules
{

   
    public class PaginatorModuleLoader : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RestPaginatorService>().As<IRestPaginatorService>().AsSelf();
            base.Load(builder);
        }
    }
}
