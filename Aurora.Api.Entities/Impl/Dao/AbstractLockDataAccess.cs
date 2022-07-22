using Aurora.Api.Entities.Impl.Entities;
using Aurora.Api.Entities.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Aurora.Api.Entities.Impl.Dao
{
    public class AbstractLockDataAccess<TId, TEntity, TDbContext> : AbstractDataAccess<TId, TEntity, TDbContext>
        where TEntity : BaseLockEntity<TId>, IBaseEntity<TId> where TDbContext : DbContext
    {
        public AbstractLockDataAccess(IDbContextFactory<TDbContext> dbContext, ILogger<TEntity> logger) : base(
            dbContext,
            logger)
        {
        }

        public override Task<TEntity> Update(TEntity entity)
        {
            return UpdateWithLock(entity);
        }

        private async Task<TEntity> UpdateWithLock(TEntity entity)
        {
            await using var dbContext = await DbContext.CreateDbContextAsync();
            //    await using var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);
            dbContext.Entry(entity).State = EntityState.Modified;
            bool saveFailed;

            do
            {
                saveFailed = false;

                try
                {
                    await dbContext.BulkSaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store 
                    await ex.Entries.Single().ReloadAsync();
                }

            } while (saveFailed);

            // await transaction.CommitAsync();

            return entity;
        }
    }
}
