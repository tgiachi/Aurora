using Aurora.Api.Interfaces.Services;
using Aurora.Api.Services;
using Autofac;

namespace Aurora.Api.MethodEx
{
    public static class TaskQueueServiceExtension
    {
        public static ContainerBuilder AddTaskQueueService(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<TaskQueueService>().As<ITaskQueueService>().AsSelf();

            return containerBuilder;
        }
    }
}
