using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.KampDigi.Domain.Entities
{
    public enum OccupantType
    {
        None,
        Owner,
    }
    public enum Gender
    {
        Male,
        Female
    }
    public class Resident : BaseEntity
    {
        [Required]
        [StringLength(StringLengthConstant.StringNameLength)]
        public string Name { get; set; }
        [Required]
        [StringLength(StringLengthConstant.StringIdentityLength)]
        public string IdentityNumber { get; set; }
        [Required]
        [StringLength(StringLengthConstant.StringContactNumber)]
        public string ContactNumber { get; set; }
        [Required]
        public OccupantType OccupantType { get; set; }
        [Required]
        public int TotalOccupant { get; set; }
        [DataType(DataType.Text)]
        public string IdentityPhoto { get; set; }
        [DataType(DataType.Text)]
        public string IdentityFamilyPhoto { get; set; }
        public Gender Gender { get; set; }
        [Required]
        [StringLength(StringLengthConstant.StringNameLength)]
        public string EmergencyCallName { get; set; }
        [Required]
        public string EmergencyCallNumber { get; set; }
        [Required]
        public Relationship EmergencyCallRelation { get; set; }
        
        [ForeignKey(nameof(House))]
        public Guid HouseId { get; set; }
        public bool IsOccupant { get; set; }
        
        public virtual House House { get; set; }
        public virtual ICollection<ResidentFamily> ResidentFamilies { get; set; }
        public virtual ICollection<GuestBook> GuestBooks { get; set; }

    }
}
