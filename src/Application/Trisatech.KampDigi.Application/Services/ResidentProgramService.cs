using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Application.Models.ResidentProgram;
using Trisatech.KampDigi.Domain;

namespace Trisatech.KampDigi.Application.Interfaces;

public class ResidentProgramService : BaseDbService, IResidentProgramService
{
   public ResidentProgramService(KampDigiContext dbContext) : base(dbContext)
   {
   }

   public async Task<ResidentProgramModel> Add(ResidentProgramModel req)
   {
      await Db.AddAsync(req);
      await Db.SaveChangesAsync();

      return req;
   }

   public Task<bool> Delete(Guid id)
   {
      throw new NotImplementedException();
   }

   public Task<List<ResidentProgramModel>> Get(int limit, int offset, string keyword)
   {
      throw new NotImplementedException();
   }

   public Task<ResidentProgramModel> Get(Guid id)
   {
      throw new NotImplementedException();
   }

   public Task<ResidentProgramModel> Get(Expression<Func<ResidentProgramModel, bool>> func)
   {
      throw new NotImplementedException();
   }

   public async Task<List<ResidentProgramModel>> GetAll()
   {
      var data = await (from a in Db.ResidentPrograms
                        select new ResidentProgramModel
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

   public Task<ResidentProgramModel> Update(ResidentProgramModel obj)
   {
      throw new NotImplementedException();
   }
}
