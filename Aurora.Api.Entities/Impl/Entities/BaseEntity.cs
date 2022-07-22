using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Aurora.Api.Entities.Interfaces.Entities;

namespace Aurora.Api.Entities.Impl.Entities
{
    public class BaseEntity : IBaseEntity<long>
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("created_date")] public DateTime CreateDateTime { get; set; }
        [Column("updated_date")] public DateTime UpdatedDateTime { get; set; }
    }
}
