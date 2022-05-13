using Trisatech.KampDigi.Domain.Entities;
namespace Trisatech.KampDigi.Application.Models
{
   public class ResidentProgramModel
   {
      public Guid? Id { get; set; }
      public int Year { get; set; }
      public string ProgramSubject { get; set; }
      public string Title { get; set; }
      public string Desc { get; set; }
      public decimal Cost { get; set; }
      public ProgramPeriod ProgramPeriod { get; set; }
      public DateTime? StartDate { get; set; }
      public DateTime? EndDate { get; set; }
      public Guid PersonInChargeId { get; set; }
      public string PersonInChargeName { get; set; }
      public virtual Trisatech.KampDigi.Domain.Entities.Resident PersonInCharge { get; set; }

      public Guid CreatedBy { get; set; }
      public Guid UpdatedBy { get; set; }
      public ResidentProgram ConvertToDbModel()
      {
         return new ResidentProgram()
         {
            Id = (this.Id == null) ? Guid.NewGuid() : this.Id.Value,
            Year = this.Year,
            ProgramSubject = this.ProgramSubject,
            Title = this.Title,
            Desc = this.Desc,
            Cost = this.Cost,
            ProgramPeriod = this.ProgramPeriod,
            StartDate = this.StartDate,
            EndDate = this.EndDate,
            PersonInChargeId = this.PersonInChargeId,
            CreatedBy = this.CreatedBy,
            UpdatedBy = this.UpdatedBy
         };
      }

   }

}

