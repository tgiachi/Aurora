using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.RabbitMq.Interfaces
{
    public interface IAuroraRabbitMqService : IDisposable
    {
        Task Subscribe<TEntity>(string queueName, Action<TEntity> action);

        Task Publish<TEntity>(string queueName, TEntity entity);
    }
}
