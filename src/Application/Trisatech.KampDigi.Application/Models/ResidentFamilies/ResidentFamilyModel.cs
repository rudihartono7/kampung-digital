using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Models.ResidentFamilies
{
    public class ResidentFamilyModel
    {
        public ResidentFamilyModel()
        {
            
        }
        public ResidentFamilyModel(ResidentFamily item)
        {
            Id = this.Id;
            Name = this.Name;
            Gender = this.Gender;
            Age = this.Age;
            Relationship = this.Relationship;
            HeadOfFamilyId = this.HeadOfFamilyId;
            
        }
        
        public Guid? Id {get; set;}
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public Relationship Relationship { get; set; }
        public Guid HeadOfFamilyId { get; set; }
        public string HeadOfFamilyName { get; set; }

    }
}
