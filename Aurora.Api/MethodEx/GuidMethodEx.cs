using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Api.MethodEx
{
    public static class GuidMethodEx
    {
        public static string GenerateNewUid(this Guid value)
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
