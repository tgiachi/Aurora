namespace Aurora.Api.Entities.Interfaces.Seeds
{
    public interface IDbSeed
    {
        Task<bool> Seed();
    }
}
