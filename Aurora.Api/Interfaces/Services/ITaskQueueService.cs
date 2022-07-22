namespace Aurora.Api.Interfaces.Services
{
    public interface ITaskQueueService
    {
        Task<IEnumerable<TResult>> ProcessTasksQueue<TEntity, TResult>(List<TEntity> objects,
            Func<TEntity, Task<TResult>> executeFunc,
            int maxConcurrentTasks = 10);
    }
}
