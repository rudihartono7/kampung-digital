using Microsoft.AspNetCore.Http;
using Trisatech.KampDigi.Domain.Entities;
namespace Trisatech.KampDigi.Application.Models
{
   public class ResidenceModel
   {
      public ResidenceModel()
      {
         
      }
      public Guid? Id { get; set; }
      public string Name { get; set; }
      public string Address { get; set; }
      public double Latitude { get; set; }
      public double Longitude { get; set; }
      public string ImageUrl { get; set; }
      public IFormFile? GambarFile { get; set; }
      public string GMapLink { get; set; }
      public Guid PersonInCharge { get; set; }
      public string PersonInChargeName { get; set; }
      
      public virtual Trisatech.KampDigi.Domain.Entities.Resident Resident { get; set; }
      public virtual ICollection<House> Houses { get; set; }

      public Residence ConvertToDbModel()
      {
         return new Residence()
         {
            Id = (this.Id == null) ? Guid.NewGuid() : this.Id.Value,
            Name = this.Name,
            Address = this.Address,
            GMapLink = this.GMapLink,
            Latitude = this.Latitude,
            Longitude = this.Longitude,
            ImageUrl = this.ImageUrl,
            PersonInCharge = this.PersonInCharge
         };
      }

   }
}