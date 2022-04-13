using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;
public class ResidentFamilyService : BaseDbService, IResidentFamilyService {
    public ResidentFamilyService(KampDigiContext dbContext) : base(dbContext)
    {
    }

    public async Task<ResidentFamily> Add(ResidentFamily obj)
    {
        if(await Db.ResidentFamilies.AnyAsync(x=>x.Id == obj.Id)){
            throw new InvalidOperationException($"Resident Family with ID {obj.Id} is already exist");
        }

        await Db.AddAsync(obj);
        await Db.SaveChangesAsync();

        return obj;
    }

    public async Task<bool> Delete(Guid id)
    {
        var family = await Db.ResidentFamilies.FirstOrDefaultAsync(x=>x.Id == id);

        if(family == null) {
            throw new InvalidOperationException($"Resident Family with ID {id} doesn't exist");
        }

        Db.Remove(family);
        await Db.SaveChangesAsync();

        return true;
    }

    public Task<List<ResidentFamily>> Get(int limit, int offset, string keyword)
    {
        throw new NotImplementedException();
    }

    public Task<ResidentFamily> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ResidentFamily> Get(Expression<Func<ResidentFamily, bool>> func)
    {
        throw new NotImplementedException();
    }

    public Task<List<ResidentFamily>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<ResidentFamily> Update(ResidentFamily obj)
    {
        if(obj == null)
        {
            throw new ArgumentNullException("Id cannot be null");
        }

        var family= await Db.ResidentFamilies.FirstOrDefaultAsync(x=>x.Id == obj.Id);

        if(family == null) {
            throw new InvalidOperationException($"Family with ID {obj.Id} doesn't exist in database");
        }

        family.Name = obj.Name;
        family.Gender = obj.Gender;
        family.Age = obj.Age;
        family.Relationship = obj.Relationship;


        Db.Update(family);
        await Db.SaveChangesAsync();

        return family;
    }

    async Task<List<ResidentFamilyModel>> IResidentFamilyService.GetId(Guid IdHead)
    {
        var result = await (from a in Db.ResidentFamilies where a.HeadOfFamilyId == IdHead
        select new ResidentFamilyModel 
        {
            Name = a.Name,
            Gender = a.Gender,
            Age = a.Age,
            Relationship = a.Relationship,
            HeadOfFamilyId = a.HeadOfFamilyId       
        }).ToListAsync();

        return result;
    }


}
