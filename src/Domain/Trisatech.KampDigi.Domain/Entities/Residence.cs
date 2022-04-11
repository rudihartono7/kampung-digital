using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.KampDigi.Domain.Entities
{
    /// <summary>
    /// Informasi kampung yang dimunculkan di halaman landing page
    /// </summary>
    public class Residence : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ImageUrl { get; set; }
        public string GMapLink { get; set; }
        [ForeignKey(nameof(Entities.Resident))]
        public Guid PersonInCharge { get; set; }
        public virtual Resident Resident { get; set; }
        public virtual ICollection<House> Houses { get; set; }
    }
}
