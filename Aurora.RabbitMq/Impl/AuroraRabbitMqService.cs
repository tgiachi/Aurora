using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Api.Interfaces.Services;
using Aurora.Api.Services.Base;
using Aurora.RabbitMq.Data;
using Aurora.RabbitMq.Interfaces;
using EasyNetQ;
using Microsoft.Extensions.Logging;

namespace Aurora.RabbitMq.Impl
{
    public class AuroraRabbitMqService : AbstractBaseService<AuroraRabbitMqService>, IAuroraRabbitMqService
    {
        private readonly IBus _bus;
        public AuroraRabbitMqService(IEventBusService eventBusService, ILogger<AuroraRabbitMqService> logger, AuroraRabbitMqConfig config) : base(eventBusService, logger)
        {
            _bus = RabbitHutch.CreateBus(config.ConnectionString);
        }

        public void Dispose()
        {
            _bus.Dispose();
        }

        public Task Subscribe<TEntity>(string queueName, Action<TEntity> action)
        {
           return _bus.PubSub.SubscribeAsync<TEntity>(queueName, action);
        }

        public Task Publish<TEntity>(string queueName, TEntity entity)
        {
            return _bus.PubSub.PublishAsync(entity, queueName);
        }
    }
}
