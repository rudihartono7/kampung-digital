using System.ComponentModel.DataAnnotations;

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
