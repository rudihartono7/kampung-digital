using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Models.ResidentProgram
{
   public class ResidentProgramVModel
   {
      public int Year { get; set; }
      public string ProgramSubject { get; set; }
      public string Title { get; set; }
      public string Desc { get; set; }
      public DateTime? StartDate { get; set; }
      public DateTime? EndDate { get; set; }
   }
}
