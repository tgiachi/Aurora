using Aurora.Api.Attributes;
using Aurora.Api.Utils;
using Autofac;

namespace Aurora.Api.MethodEx
{
    public static class ScanModulesMethodEx
    {
        public static ContainerBuilder ScanModules(this ContainerBuilder containerBuilder)
        {
            AssemblyUtils.GetAttribute<ModuleLoaderAttribute>().ForEach(m =>
            {
                var module = Activator.CreateInstance(m) as Module;
                containerBuilder.RegisterModule(module);
            });

            return containerBuilder;
        }
    }
}
