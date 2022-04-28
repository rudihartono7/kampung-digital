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
      var data = await Db.ResidentPrograms.Where(x => x.ProgramPeriod == period).ToListAsync();
      return data;
   }

   public async Task<List<ResidentProgram>> GetByQuery(string query)
   {
      var datas = await Db.ResidentPrograms.FromSqlRaw(query).ToListAsync();
      return datas;
   }

   public async Task<ResidentProgramModel> GetDetail(Guid Id)
   {
      var data = await (from rProg in Db.ResidentPrograms
                        join resident in Db.Residents
                  on rProg.PersonInChargeId equals resident.Id
                        where rProg.Id == Id
                        select new ResidentProgramModel
                        {
                           Year = rProg.Year,
                           Title = rProg.Title,
                           Id = rProg.Id,
                           StartDate = rProg.StartDate,
                           EndDate = rProg.EndDate,
                           PersonInChargeName = resident.Name,
                           Cost = rProg.Cost,
                           ProgramPeriod = rProg.ProgramPeriod,
                           Desc = rProg.Desc,
                           ProgramSubject = rProg.ProgramSubject
                        }).FirstOrDefaultAsync();
      return data;
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
      residentProgram.UpdatedBy = req.UpdatedBy;
      residentProgram.AuditActivty = Trisatech.KampDigi.Domain.Entities.AuditActivtyType.UPDATE;
      residentProgram.PersonInChargeId = req.PersonInChargeId;
      residentProgram.Cost = req.Cost;
      residentProgram.Desc = req.Desc;
      residentProgram.EndDate = req.EndDate;
      residentProgram.ProgramPeriod = req.ProgramPeriod;
      residentProgram.ProgramSubject = req.ProgramSubject;
      residentProgram.StartDate = req.StartDate;
      residentProgram.Title = req.Title;
      residentProgram.Year = req.Year;

      Db.Update(residentProgram);
      await Db.SaveChangesAsync();

      return residentProgram;
   }
}