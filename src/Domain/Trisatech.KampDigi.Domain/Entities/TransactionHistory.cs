using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.KampDigi.Domain.Entities
{
    public enum TransactionType
    {
        Credit,
        Debit
    }
    public enum CreditSource
    {
        ResidentBill,
        Donation,
        Other
    }
    public class TransactionHistory: BaseEntity
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        public string Note { get; set; }
        [DataType(DataType.Text)]
        public string Evidence { get; set; }
        public bool IsResidentBill { get; set; }
        public decimal Nominal { get; set; }
        public TransactionType Type { get; set; }
        public CreditSource CreditSource { get; set; }
        [ForeignKey(nameof(Entities.ResidentProgram))]
        public Guid? ResidentProgramId { get; set; }
        public virtual ResidentProgram ResidentProgram { get; set; }
    }
}
