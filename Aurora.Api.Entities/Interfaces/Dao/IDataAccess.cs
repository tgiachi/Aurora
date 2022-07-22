using Aurora.Api.Entities.Interfaces.Entities;

namespace Aurora.Api.Entities.Interfaces.Dao
{
    public interface IDataAccess<TId, TEntity> where TEntity : class, IBaseEntity<TId>
    {
        Task<long> Count();

        Task<List<TEntity>> FindAll();

        Task<TEntity> Insert(TEntity entity);

        Task<List<TEntity>> Insert(List<TEntity> entity);

        Task<List<TEntity>> InsertBulk(List<TEntity> entities);

        Task<TEntity> Update(TEntity entity);
        Task<List<TEntity>> Update(List<TEntity> entities);

        Task<TEntity> InsertOrUpdate(TEntity entity);

        Task<TEntity> FindById(TId id);

        Task<bool> Delete(TEntity entity);

        Task<bool> Delete(TId id);

        IQueryable<TEntity> Query(Func<IQueryable<TEntity>, IQueryable<TEntity>> func);

        IQueryable<TEntity> Query(Func<IQueryable<TEntity>, IQueryable<TEntity>> func, int skip, int take);

        Task<List<TEntity>> QueryAsList(Func<IQueryable<TEntity>, IQueryable<TEntity>> func);

        Task<List<TGenericEntity>> QueryAsList<TGenericEntity>(
            Func<IQueryable<TEntity>, IQueryable<TGenericEntity>> func);

        Task<TEntity> QueryAsSingle(Func<IQueryable<TEntity>, IQueryable<TEntity>> func);

        Task<List<TEntity>> BulkGetById(params TId[] ids);

        Task<long> Count(Func<IQueryable<TEntity>, IQueryable<TEntity>> func);
    }
}
