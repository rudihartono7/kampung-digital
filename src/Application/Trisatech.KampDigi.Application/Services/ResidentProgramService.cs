using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Application.Models.ResidentProgram;
using Trisatech.KampDigi.Domain;

namespace Trisatech.KampDigi.Application.Interfaces;

public class ResidentProgramService : BaseDbService, IResidentProgramService {
   public ResidentProgramService(KampDigiContext dbContext) : base(dbContext)
   {
   }

   public Task<ResidentProgramModel> Add(ResidentProgramModel obj)
   {
      throw new NotImplementedException();
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

   public Task<List<ResidentProgramModel>> GetAll()
   {
      throw new NotImplementedException();
   }

   public Task<ResidentProgramModel> Update(ResidentProgramModel obj)
   {
      throw new NotImplementedException();
   }
}
