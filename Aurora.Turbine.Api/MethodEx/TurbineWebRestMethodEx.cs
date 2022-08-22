using Aurora.Turbine.Api.Modules;
using Autofac;

namespace Aurora.Turbine.Api.MethodEx
{
    public static class TurbineWebRestMethodEx
    {
        public static ContainerBuilder AddTurbineRestServices(this ContainerBuilder value)
        {
            value.RegisterModule<PaginatorModuleLoader>();
            
            return value;
        }
    }
}
