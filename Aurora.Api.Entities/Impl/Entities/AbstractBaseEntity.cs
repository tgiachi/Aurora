using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Api.Entities.Interfaces.Entities;

namespace Aurora.Api.Entities.Impl.Entities
{
    public class AbstractBaseEntity<TId> : IBaseEntity<TId>
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TId Id { get; set; }

        [Column("created_date")] public DateTime CreateDateTime { get; set; }
        [Column("updated_date")] public DateTime UpdatedDateTime { get; set; }
    }
}
