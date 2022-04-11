using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.KampDigi.Domain.Entities
{
    public enum Relationship
    {
        Husband,
        Wife,
        Child,
        Sibling,
        Father,
        Mother,
        GrandFather,
        GrandMother,
        GrandChild,
        Friend
    }
    public class ResidentFamily : BaseEntity
    {
        [Required]
        [StringLength(StringLengthConstant.StringNameLength)]
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public Relationship Relationship { get; set; }
        [ForeignKey(nameof(Resident))]
        public Guid HeadOfFamilyId { get; set; }
        public virtual Resident HeadOfFamily { get; set; }
    }
}
