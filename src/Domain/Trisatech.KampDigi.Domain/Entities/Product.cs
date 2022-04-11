using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.KampDigi.Domain.Entities
{
    public class Product : BaseEntity
    {
        [Required]
        [StringLength(StringLengthConstant.StringNameLength)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        [StringLength(255)]
        public string PublicLink { get; set; }
        [Required]
        [StringLength(StringLengthConstant.StringNameLength)]
        public string SellerName { get; set; }
        [Required]
        public string WhatsappNumber { get; set; }
        public string ImageUrl { get; set; }
        public string Desc { get; set; }
    }
}
