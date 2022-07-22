using Autofac;

namespace Aurora.Api.MethodEx
{
    public static class EnvModeMethodEx
    {
        public static ContainerBuilder ForceSetEnv(this ContainerBuilder containerBuilder, string mode)
        {
            Environment.SetEnvironmentVariable("MODE", mode);
            return containerBuilder;
        }
    }
}
