using System.ComponentModel.DataAnnotations;

namespace Trisatech.KampDigi.Domain.Entities
{
    public enum DonationType
    {
        Residence,
        Charity,
        Mosque,
        AppDevelopment
    }
    public enum DonationMethod
    {
        Cash,
        Transfer
    }
    public class Donation : BaseEntity
    {
        [Required]
        public DonationType Type { get; set; }
        [Required]
        public DonationMethod Method { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [StringLength(255)]
        public string Note { get; set; }
        [Required]
        [StringLength(StringLengthConstant.StringContactNumber)]
        public string ToNumber { get; set; }
        [Required]
        [StringLength(StringLengthConstant.StringNameLength)]
        public string ToName { get; set; }
        [Required]
        public decimal Nominal { get; set; }
        [DataType(DataType.Text)]
        public string Evidence { get; set; }
    }
}
