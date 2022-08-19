using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.RabbitMq.Data;
using Aurora.RabbitMq.Impl;
using Aurora.RabbitMq.Interfaces;
using Autofac;

namespace Aurora.RabbitMq.MethodEx
{
    public static class AuroraRabbitMqBuilder
    {
        public static ContainerBuilder AddRabbitMq(this ContainerBuilder builder, string connectionString)
        {
            var config = new AuroraRabbitMqConfig() {ConnectionString = connectionString};
            builder.RegisterInstance(config);

            builder.RegisterType<AuroraRabbitMqService>().As<IAuroraRabbitMqService>().AsSelf().SingleInstance();

            return builder;
        }
    }
}
