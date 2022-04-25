using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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
      await Db.AddAsync(req);
      await Db.SaveChangesAsync();

      return req;
   }

   public Task<bool> Delete(Guid id)
   {
      throw new NotImplementedException();
   }

   public Task<List<ResidentProgram>> Get(int limit, int offset, string keyword)
   {
      throw new NotImplementedException();
   }

   public Task<ResidentProgram> Get(Guid id)
   {
      throw new NotImplementedException();
   }

   public Task<ResidentProgram> Get(Expression<Func<ResidentProgram, bool>> func)
   {
      throw new NotImplementedException();
   }

   public async Task<List<ResidentProgram>> GetAll()
   {
      var data = await (from a in Db.ResidentPrograms
                        select new ResidentProgram
                        {
                           Year = a.Year,
                           Title = a.Title,
                           Desc = a.Desc,
                           StartDate = a.StartDate,
                           EndDate = a.EndDate,
                           ProgramSubject = a.ProgramSubject,
                           Cost = a.Cost,
                           PersonInCharge = a.PersonInCharge,
                           PersonInChargeId = a.PersonInChargeId,
                           ProgramPeriod = a.ProgramPeriod
                        }).ToListAsync();
      return data;
   }

   public Task<ResidentProgram> Update(ResidentProgram obj)
   {
      throw new NotImplementedException();
   }
}
