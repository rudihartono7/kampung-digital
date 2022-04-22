using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Models
{
   public class HouseDetailModel : HouseModel
   {
      public string HeadOfFamilyName { get; set; }
      public ICollection<ResidentFamily> FamilyMember { get; set; }
   }
}