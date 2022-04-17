using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;
public class HouseService : BaseDbService, IHouseService
{
   public HouseService(KampDigiContext dbContext) : base(dbContext)
   {
   }

   public async Task<House> Add(House obj)
   {
      if (await Db.Houses.AnyAsync(x => x.Id == obj.Id))
      {
         throw new InvalidOperationException($"Resident House with ID {obj.Id} is already exist");
      }

      obj.AuditActivty = Trisatech.KampDigi.Domain.Entities.AuditActivtyType.INSERT;
      obj.CreatedBy = Guid.NewGuid();
      obj.UpdatedBy = obj.CreatedBy;
      obj.CreatedDate = DateTime.Now;
      await Db.AddAsync(obj);
      await Db.SaveChangesAsync();

      return obj;
   }

   public async Task<bool> Delete(Guid id)
   {
      var house = await Db.Houses.FirstOrDefaultAsync(x => x.Id == id);
      if (house == null)
      {
         throw new InvalidOperationException($"House with ID {id} doesn't exist");
      }
      Db.Remove(house);
      await Db.SaveChangesAsync();

      return true;
   }

   public Task<List<House>> Get(int limit, int offset, string keyword)
   {
      throw new NotImplementedException();
   }

   public async Task<House> Get(Guid id)
   {
      var house = await Db.Houses.FirstOrDefaultAsync(x=> x.Id == id);
      return house;
   }

   public Task<House> Get(Expression<Func<House, bool>> func)
   {
      throw new NotImplementedException();
   }

   public async Task<List<House>> GetAll()
   {
      return await Db.Houses.ToListAsync();
   }

   public async Task<House> Update(House obj)
   {
      if (obj == null)
      {
         throw new ArgumentNullException("Id cannot be null");
      }

      var house = await Db.Houses.FirstOrDefaultAsync(x => x.Id == obj.Id);

      if (house == null)
      {
         throw new InvalidOperationException($"house with ID {obj.Id} doesn't exist in database");
      }

      house.Number = obj.Number;
      house.Order = obj.Order;
      house.Status = obj.Status;
      house.Type = obj.Type;
      house.AuditActivty = Trisatech.KampDigi.Domain.Entities.AuditActivtyType.UPDATE;

      Db.Update(house);
      await Db.SaveChangesAsync();

      return house;
   }
}
