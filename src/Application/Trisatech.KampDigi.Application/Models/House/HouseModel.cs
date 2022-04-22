using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Models
{
   public class HouseModel
   {
      public Guid? Id { get; set; }
      public int Order { get; set; }

      public string Number { get; set; }
      public HouseType Type { get; set; }
      public HouseStatus Status { get; set; }
      public Guid? ResidenceId { get; set; }


      public House ConvertToDbModel()
      {
         return new House()
         {
            Id = (this.Id == null) ? Guid.NewGuid() : this.Id.Value,
            Order = this.Order,
            Number = this.Number,
            Type = this.Type,
            Status = this.Status,
            ResidenceId = this.ResidenceId
         };
      }

   }
}
