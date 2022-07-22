using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aurora.Turbine.Api.Data.Rest
{
    public class IamVerifyData
    {

        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
