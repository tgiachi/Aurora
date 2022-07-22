using Aurora.Api.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Aurora.Api.Services
{
    public class EventBusService : IEventBusService
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public EventBusService(ILogger<EventBusService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task PublishEvent<TEntity>(TEntity entity)
        {
            _logger.LogDebug("Sending message type {Entity}", entity.GetType().Name);
            await _mediator.Publish(entity);
        }

        public async Task<TOutEntity> Send<TInputEntity, TOutEntity>(TInputEntity inputEntity)
        {
            _logger.LogDebug("Sending message type {Entity} and except {OutEntity}", inputEntity.GetType().Name,
                typeof(TOutEntity).Name);
            var outEntity = await _mediator.Send(inputEntity);

            if (outEntity != null)
            {
                return (TOutEntity)outEntity;
            }

            return default;
        }
    }
}
