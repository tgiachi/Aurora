namespace Aurora.Api.Entities.Interfaces.Services
{
    public interface IDbSeedService
    {
        public Task<bool> ExecuteDbSeeds();
    }
}
