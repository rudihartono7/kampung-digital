using System.ComponentModel.DataAnnotations.Schema;

namespace Trisatech.KampDigi.Domain.Entities;
public class ResidentBillDetailInfo : BaseEntity {
        [ForeignKey(nameof(ResidentBillBaseInfo))]
        public Guid ResidentBillBaseInfoId { get; set; }
        public decimal Nominal { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }

        public virtual ResidentBillBaseInfo ResidentBillBaseInfo { get; set; } 
}
