using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.KampDigi.Domain.Entities
{
    public class Publication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Slag { get; set; }
        [Required]
        public DateTime PublishDate { get; set; }
        public string ImageLink { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Content { get; set; }
        [Required]
        [StringLength(StringLengthConstant.StringNameLength)]
        public string Writer { get; set; }
        public string Source { get; set; }
    }
}
