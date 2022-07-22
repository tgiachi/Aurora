using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Api.Entities.Impl.Entities
{
    public class BaseGuidEntity : AbstractBaseEntity<Guid>
    {
        public BaseGuidEntity()
        {
            Id = Guid.Empty;
        }
    }
}
