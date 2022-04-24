using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;

public class ResidentProgramService : BaseDbService, IResidentProgramService
{
   public ResidentProgramService(KampDigiContext dbContext) : base(dbContext)
   {
   }

   public async Task<ResidentProgram> Add(ResidentProgram req)
   {
      req.CreatedDate = DateTime.Now;
      req.AuditActivty = Trisatech.KampDigi.Domain.Entities.AuditActivtyType.INSERT;
      await Db.AddAsync(req);
      await Db.SaveChangesAsync();

      return req;
   }

   public async Task<bool> Delete(Guid Id)
   {
      var residentProgram = await Db.ResidentPrograms.FirstOrDefaultAsync(x => x.Id == Id);
      if (residentProgram == null)
      {
         throw new InvalidOperationException($"Resident Program with ID {Id} doesn't exist");
      }
      Db.Remove(residentProgram);
      await Db.SaveChangesAsync();

      return true;
   }

   public async Task<List<ResidentProgram>> GetByProgram(ProgramPeriod? period)
   {
      var data = await Db.ResidentPrograms.Where(x=> x.ProgramPeriod == period).ToListAsync();
      return data;
   }

   public async Task<List<ResidentProgram>> GetByQuery(string query)
   {
      var datas = await Db.ResidentPrograms.FromSqlRaw(query).ToListAsync();
      return datas;
   }

   public Task<ResidentProgramModel> GetDetail(Guid Id)
   {
      throw new NotImplementedException();
   }

   public async Task<ResidentProgram> Update(ResidentProgram req)
   {
      if (req == null)
      {
         throw new ArgumentNullException("No request Sent !");
      }

      var residentProgram = await Db.ResidentPrograms.FirstOrDefaultAsync(x => x.Id == req.Id);

      if (residentProgram == null)
      {
         throw new InvalidOperationException($"residentProgram with ID {req.Id} doesn't exist in database");
      }

      residentProgram.UpdatedDate = DateTime.Now;
      residentProgram.AuditActivty = Trisatech.KampDigi.Domain.Entities.AuditActivtyType.UPDATE;

      Db.Update(residentProgram);
      await Db.SaveChangesAsync();

      return residentProgram;
   }
}