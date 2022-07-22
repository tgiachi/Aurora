using Aurora.Api.Entities.Interfaces.Dao;
using Aurora.Api.Entities.Interfaces.Entities;
using Aurora.Api.Entities.Interfaces.Seeds;
using Microsoft.Extensions.Logging;

namespace Aurora.Api.Entities.Impl.Seeds
{
    public class AbstractDbSeed<TId, TEntity> : IDbSeed where TEntity : class, IBaseEntity<TId>
    {
        protected ILogger _logger;

        public AbstractDbSeed(IDataAccess<TId, TEntity> dao, ILogger<AbstractDbSeed<TId, TEntity>> logger)
        {
            Dao = dao;
            _logger = logger;
        }

        protected IDataAccess<TId, TEntity> Dao { get; }

        public virtual Task<bool> Seed()
        {
            return Task.FromResult(true);
        }
    }
}
