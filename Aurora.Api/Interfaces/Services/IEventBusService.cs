namespace Aurora.Api.Interfaces.Services
{
    public interface IEventBusService
    {
        Task PublishEvent<TEntity>(TEntity entity);

        Task<TOutEntity> Send<TInputEntity, TOutEntity>(TInputEntity inputEntity);
    }
}
