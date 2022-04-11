using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.KampDigi.Domain.Entities
{
    public class GuestBook : BaseEntity
    {
        [Required]
        [StringLength(StringLengthConstant.StringNameLength)]
        public string Name { get; set; }
        [Required]
        [StringLength(StringLengthConstant.StringContactNumber)]
        public string ContactNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [StringLength(255)]
        public string Necessity { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        [ForeignKey(nameof(Resident))]
        public Guid GuestToId { get; set; }
        public virtual Resident GuestTo { get; set; }

    }
}
