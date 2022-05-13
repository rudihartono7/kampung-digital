using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;
public interface IResidentProgramService 
{
   Task<ResidentProgram> Add (ResidentProgram req);
   Task<Boolean> Delete (Guid Id);
   Task<ResidentProgram> Update (ResidentProgram req);
   Task<List<ResidentProgram>> GetByQuery (string query);
   Task<List<ResidentProgram>> GetByProgram (ProgramPeriod? period);
   Task<ResidentProgramModel> GetDetail (Guid Id);
}
