namespace Trisatech.KampDigi.Domain.Entities;
public class ResidentBillBaseInfo : BaseEntity {
        public int Year { get; set; }
        public decimal Nominal { get; set; }
        public DateTime MontlyBillOpenDate { get; set; }
        public int DueDateNumber { get; set; }

        public virtual ICollection<ResidentBillDetailInfo> ResidentBillDetailInfos { get; set; }
}
