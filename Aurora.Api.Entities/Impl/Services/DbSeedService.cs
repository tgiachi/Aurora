using System.Reflection;
using Aurora.Api.Entities.Attributes;
using Aurora.Api.Entities.Interfaces.Seeds;
using Aurora.Api.Entities.Interfaces.Services;
using Aurora.Api.Utils;
using Autofac;

using Microsoft.Extensions.Logging;

namespace Aurora.Api.Entities.Impl.Services
{
    public class DbSeedService : IDbSeedService
    {
        private readonly ILifetimeScope _container;

        private readonly List<Type> _dbSeeds = new();
        private readonly ILogger _logger;

        public DbSeedService(ILogger<DbSeedService> logger, ILifetimeScope container)
        {
            _logger = logger;
            _container = container;

            ScanForDbSeeds();
        }

        public async Task<bool> ExecuteDbSeeds()
        {
            foreach (var dbSeed in _dbSeeds)
            {
                var seed = _container.Resolve(dbSeed) as IDbSeed;

                await seed.Seed();
            }

            return true;
        }

        private void ScanForDbSeeds()
        {
            var seedsTypes = AssemblyUtils.GetAttribute<DbSeedAttribute>();
            var unOrderedSeeds = new Dictionary<int, Type>();
            foreach (var seedsType in seedsTypes)
            {
                var attribute = seedsType.GetCustomAttribute<DbSeedAttribute>();
                _logger.LogInformation("Found seed {SeedName} with order: {Order}", seedsType.Name, attribute.Order);

                unOrderedSeeds.Add(attribute.Order, seedsType);
            }

            _dbSeeds.AddRange(unOrderedSeeds.OrderBy(s => s.Key).Select(s => s.Value).ToList());
        }
    }
}
