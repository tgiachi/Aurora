using System.Text.Json.Serialization;
using Aurora.Api.Entities.Interfaces.Dto;

namespace Aurora.Api.Entities.Impl.Dto
{
    public class BaseDto : IBaseDto<long>
    {
        public long Id { get; set; }

        [JsonPropertyName("created")] public DateTime CreateDateTime { get; set; }
        [JsonPropertyName("updated")] public DateTime UpdatedDateTime { get; set; }
    }
}
