using Aurora.Api.Interfaces.Services;
using Aurora.Api.Services;
using Aurora.Api.Utils;
using Autofac;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace Aurora.Api.MethodEx
{
    public static class EventBusServiceMethodEx
    {
        public static ContainerBuilder AddEventBus(this ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterAssemblyTypes(typeof(IRequest<>).Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IRequest<>)))
                .AsImplementedInterfaces();

            containerBuilder
                .RegisterAssemblyTypes(typeof(IRequestHandler<>).Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IRequestHandler<>)))
                .AsImplementedInterfaces();

            containerBuilder
                .RegisterAssemblyTypes(typeof(INotificationHandler<>).Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(INotificationHandler<>)))
                .AsImplementedInterfaces();

            containerBuilder.RegisterMediatR(AssemblyUtils.GetAppAssemblies());

            containerBuilder.RegisterType<EventBusService>().As<IEventBusService>().AsSelf().SingleInstance();


            return containerBuilder;
        }
    }
}
