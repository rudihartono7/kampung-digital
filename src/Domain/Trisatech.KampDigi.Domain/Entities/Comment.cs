using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.KampDigi.Domain.Entities
{
    public class Comment : BaseEntity
    {   
        [Required]
        public string Desc { get; set; }
        [ForeignKey(nameof(Post))]
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
