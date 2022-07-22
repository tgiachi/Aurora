using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Aurora.Api.Entities.Interfaces.Entities;

namespace Aurora.Api.Entities.Impl.Entities
{
    public class BaseUidEntity : IBaseEntity<string>
    {
        [Key]
        [Column("id", TypeName = "uuid")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Column("created_date")] public DateTime CreateDateTime { get; set; }
        [Column("updated_date")] public DateTime UpdatedDateTime { get; set; }

        public BaseUidEntity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
