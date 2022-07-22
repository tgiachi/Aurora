using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Aurora.Api.Entities.Interfaces.Dto;

namespace Aurora.Api.Entities.Impl.Dto
{
    public class AbstractGuidDtoEntity : IBaseDto<Guid>
    {
        public Guid Id { get; set; }
        [JsonPropertyName("created")] public DateTime CreateDateTime { get; set; }
        [JsonPropertyName("updated")] public DateTime UpdatedDateTime { get; set; }
    }
}
