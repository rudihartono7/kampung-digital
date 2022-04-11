using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trisatech.KampDigi.Domain.Entities
{
    public class ResidentBill : BaseEntity
    {
        [ForeignKey(nameof(Resident))]
        public Guid ResidentTo { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int Month { get; set; }
        [Required]
        public decimal Nominal { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime BillOpenDate { get; set; }
        public string Evidence { get; set; }
        public string Note { get; set; }

        public virtual Resident Resident { get; set; }
    }
}
