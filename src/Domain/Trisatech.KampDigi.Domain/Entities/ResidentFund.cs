using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.KampDigi.Domain.Entities
{
    public class ResidentFund : BaseEntity
    {
        [Required]
        public int Year { get; set; }
        public decimal BeginingBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal EndingBalance { get; set; }
        public decimal HoldBalance { get; set; }
    }
}
