using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.KampDigi.Domain.Entities
{
    public enum PostType
    {
        Report,
        Forum,
        Announcement
    }
    public class Post : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string PostSubject { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [DataType(DataType.Html)]
        public string Desc { get; set; }
        public string Image { get; set; }
        public PostType Type { get; set; }
        public bool IsResidentProgram { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
