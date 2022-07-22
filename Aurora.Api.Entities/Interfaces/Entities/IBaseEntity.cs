namespace Aurora.Api.Entities.Interfaces.Entities
{
    public interface IBaseEntity<TId>
    {
        TId Id { get; set; }

        DateTime CreateDateTime { get; set; }

        DateTime UpdatedDateTime { get; set; }
    }
}
