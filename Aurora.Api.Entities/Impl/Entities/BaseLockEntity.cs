using System.ComponentModel.DataAnnotations;

namespace Aurora.Api.Entities.Impl.Entities
{
    public class BaseLockEntity<TId> : AbstractBaseEntity<TId>
    {
        public uint xmin { get; set; }
    }
}
