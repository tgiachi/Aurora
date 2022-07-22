using Aurora.Api.Entities.Interfaces.Dao;
using Aurora.Api.Entities.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Aurora.Api.Entities.Impl.Dao
{
    public class AbstractDataAccess<TId, TEntity, TDbContext> : IDataAccess<TId, TEntity>
        where TEntity : class, IBaseEntity<TId> where TDbContext : DbContext
    {
        private readonly ILogger _logger;

        public AbstractDataAccess(IDbContextFactory<TDbContext> dbContext, ILogger<TEntity> logger)
        {
            DbContext = dbContext;
            _logger = logger;
        }

        protected IDbContextFactory<TDbContext> DbContext { get; }

        public virtual async Task<long> Count()
        {
            await using var dbContext = await DbContext.CreateDbContextAsync();
            //await using var transaction = await dbContext.Database.BeginTransactionAsync();


            _logger.LogDebug("Count for entity {EntityName}", typeof(TEntity).Name);
            var count = await dbContext.Set<TEntity>().LongCountAsync();


            return count;
        }

        public virtual async Task<List<TEntity>> FindAll()
        {
            await using var dbContext = await DbContext.CreateDbContextAsync();
            _logger.LogDebug("Find all entities {EntityName}", typeof(TEntity).Name);
            var list = await dbContext.Set<TEntity>().ToListAsync();
            // await transaction.CommitAsync();
            return list;
        }

        public virtual async Task<TEntity> Insert(TEntity entity)
        {
            await using var dbContext = await DbContext.CreateDbContextAsync();


            entity.CreateDateTime = DateTime.Now;
            entity.UpdatedDateTime = DateTime.Now;

            dbContext.Set<TEntity>().Add(entity);

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await dbContext.DisposeAsync();
                throw;
            }
            finally
            {
                await dbContext.DisposeAsync();
            }

            return entity;
        }

        public async Task<List<TEntity>> Insert(List<TEntity> entities)
        {
            await using var dbContext = await DbContext.CreateDbContextAsync();

            entities.ForEach(s =>
            {
                s.CreateDateTime = DateTime.Now;
                s.UpdatedDateTime = DateTime.Now;
                dbContext.Set<TEntity>().Add(s);
            });

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await dbContext.DisposeAsync();
                throw;
            }
            finally
            {
                await dbContext.DisposeAsync();
            }
            //   await transaction.CommitAsync();

            return entities;
        }

        public async Task<List<TEntity>> InsertBulk(List<TEntity> entities)
        {
            await using var dbContext = await DbContext.CreateDbContextAsync();
            entities.ForEach(s =>
            {
                s.CreateDateTime = DateTime.Now;
                s.UpdatedDateTime = DateTime.Now;
            });

            await dbContext.BulkInsertAsync(entities);
            await dbContext.BulkSaveChangesAsync();
            //await transaction.CommitAsync();

            return entities;
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            await using var dbContext = await DbContext.CreateDbContextAsync();
            entity.UpdatedDateTime = DateTime.Now;

            dbContext.Entry(entity).State = EntityState.Modified;

            await dbContext.SaveChangesAsync();
            //await transaction.CommitAsync();

            return entity;
        }

        public async Task<List<TEntity>> Update(List<TEntity> entities)
        {
            await using var dbContext = await DbContext.CreateDbContextAsync();
            entities.ForEach(entity =>
            {
                entity.UpdatedDateTime = DateTime.Now;
                dbContext.Entry(entity).State = EntityState.Modified;
            });

            await dbContext.SaveChangesAsync();

            return entities;

        }

        public virtual async Task<List<TEntity>> BulkGetById(params TId[] ids)
        {
            await using var dbContext = await DbContext.CreateDbContextAsync();
            return await dbContext.Set<TEntity>().Where(s => ids.ToList().Contains(s.Id)).ToListAsync();
        }

        public virtual Task<TEntity> InsertOrUpdate(TEntity entity)
        {
            return entity.Id.Equals(default(TId)) ? Insert(entity) : Update(entity);
        }

        public virtual async Task<TEntity> FindById(TId id)
        {
            await using var dbContext = await DbContext.CreateDbContextAsync();

            var result = await dbContext.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id.Equals(id));

            //   await transaction.CommitAsync();
            return result;
        }

        public virtual async Task<bool> Delete(TEntity entity)
        {
            await using var dbContext = await DbContext.CreateDbContextAsync();
            dbContext.Entry(entity).State = EntityState.Deleted;

            await dbContext.SaveChangesAsync();
            //    await transaction.CommitAsync();
            return true;
        }

        public virtual async Task<bool> Delete(TId id)
        {
            var entity = await FindById(id);
            if (entity != null)
            {
                return await Delete(entity);
            }

            return false;
        }



        public IQueryable<TEntity> Query(Func<IQueryable<TEntity>, IQueryable<TEntity>> func)
        {
            using var dbContext = DbContext.CreateDbContext();
            var query = func.Invoke(dbContext.Set<TEntity>().AsQueryable());
            return query;
        }

        public IQueryable<TEntity> Query(Func<IQueryable<TEntity>, IQueryable<TEntity>> func, int skip, int take)
        {
            using var dbContext = DbContext.CreateDbContext();
            var query = func.Invoke(dbContext.Set<TEntity>().AsQueryable()).Skip(skip).Take(take).AsQueryable();
            return query;
        }

        public async Task<List<TEntity>> QueryAsList(Func<IQueryable<TEntity>, IQueryable<TEntity>> func)
        {
            await using var dbContext = await DbContext.CreateDbContextAsync();
            var result = await func.Invoke(dbContext.Set<TEntity>().AsQueryable()).ToListAsync();
            return result;
        }

        public async Task<List<TGenericEntity>> QueryAsList<TGenericEntity>(Func<IQueryable<TEntity>, IQueryable<TGenericEntity>> func)
        {
            await using var dbContext = await DbContext.CreateDbContextAsync();
            var result = await func.Invoke(dbContext.Set<TEntity>().AsQueryable()).ToListAsync();
            return result;
        }


        public async Task<TEntity> QueryAsSingle(Func<IQueryable<TEntity>, IQueryable<TEntity>> func)
        {
            TEntity result = null;
            //await using var dbContext = await DbContext.CreateDbContextAsync();
            await using var dbContext = await DbContext.CreateDbContextAsync();
            result = await func.Invoke(dbContext.Set<TEntity>().AsQueryable()).FirstOrDefaultAsync();


            return result;
        }

        public virtual async Task<bool> Delete(List<TEntity> entities)
        {
            await using var dbContext = await DbContext.CreateDbContextAsync();
            entities.ForEach(s => dbContext.Entry(s).State = EntityState.Deleted);


            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<TCrossEntity>> QueryAsList<TCrossEntity>(
            Func<IQueryable<TCrossEntity>, IQueryable<TCrossEntity>> func) where TCrossEntity : class, IBaseEntity<TId>
        {
            await using var dbContext = await DbContext.CreateDbContextAsync();
            var result = await func.Invoke(dbContext.Set<TCrossEntity>().AsQueryable()).ToListAsync();
            return result;
        }

        public async Task<long> Count(Func<IQueryable<TEntity>, IQueryable<TEntity>> func)
        {
            await using var dbContext = await DbContext.CreateDbContextAsync();
            var query = func.Invoke(dbContext.Set<TEntity>().AsQueryable());
            var result = await query.LongCountAsync();

            return result;
        }

        public async Task<TCrossEntity> QueryAsSingle<TCrossEntity>(
            Func<IQueryable<TCrossEntity>, IQueryable<TCrossEntity>> func) where TCrossEntity : class, IBaseEntity<TId>
        {
            await using var dbContext = await DbContext.CreateDbContextAsync();
            var result = await func.Invoke(dbContext.Set<TCrossEntity>().AsQueryable()).FirstOrDefaultAsync();

            return result;
        }
    }
}
