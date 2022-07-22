namespace Aurora.Api.Entities.Interfaces.Dto
{
    public interface IBaseDto<TId>
    {
        TId Id { get; set; }

        DateTime CreateDateTime { get; set; }

        DateTime UpdatedDateTime { get; set; }
    }
}
