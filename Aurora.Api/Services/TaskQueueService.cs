using System.Collections.Concurrent;
using Aurora.Api.Interfaces.Services;
using Dasync.Collections;
using Microsoft.Extensions.Logging;

namespace Aurora.Api.Services
{
    public class TaskQueueService : ITaskQueueService
    {
        private readonly ILogger _logger;

        public TaskQueueService(ILogger<TaskQueueService> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<TResult>> ProcessTasksQueue<TEntity, TResult>(List<TEntity> objects,
            Func<TEntity, Task<TResult>> executeFunc,
            int maxConcurrentTasks = 10)
        {
            var results = new ConcurrentBag<TResult>();

            await objects.ParallelForEachAsync(async (entity, num) =>
            {
                try
                {
                    var res = await executeFunc.Invoke(entity);
                    if (res != null)
                    {
                        results.Add(res);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error during process {Num} of task type {OutEntity}: {Error}", num,
                        typeof(TResult).Name, ex.Message);
                }
            }, maxConcurrentTasks);

            return results;
        }
    }
}
