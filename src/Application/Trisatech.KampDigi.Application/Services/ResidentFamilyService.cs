using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Application.Models.Resident;
using Trisatech.KampDigi.Application.Models.ResidentFamilies;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;
public class ResidentFamilyService : BaseDbService, IResidentFamilyService
{
    public ResidentFamilyService(KampDigiContext dbContext) : base(dbContext)
    {
    }

    async Task<ResidentFamilyModel> IResidentFamilyService.FamilyAdd(ResidentFamilyModel model, Guid idCurrentUser)
    {
        if (await Db.ResidentFamilies.AnyAsync(x => x.Id == model.Id))
            {
                throw new InvalidOperationException($"Id {model.Id} sudah terdaftar");
            }

            var newFamily = new ResidentFamily
            {
                Name = model.Name,
                Gender = model.Gender,
                Age = model.Age,
                Relationship = model.Relationship,
                HeadOfFamilyId = model.HeadOfFamilyId,
                CreatedBy = idCurrentUser,
                CreatedDate = DateTime.Now,
                AuditActivty = Trisatech.KampDigi.Domain.Entities.AuditActivtyType.INSERT,
            };
            await Db.ResidentFamilies.AddAsync(newFamily);
            await Db.SaveChangesAsync();


            return model;
    }

    public async Task<bool> FamilyDelete(Guid id)
    {
            var family = Db.ResidentFamilies.FirstOrDefault(x => x.Id == id);
            if (family == null)
            {
                throw new InvalidOperationException($"Family User dengan ID {id} tidak dapat ditemukan");
            }

            Db.Remove(family);
            await Db.SaveChangesAsync();
            return true;
    }

    public async Task<ResidentFamilyModel> FamilyDetail(Guid idHead)
    {
        var result = await (from a in Db.ResidentFamilies 
                            join b in Db.Residents on a.HeadOfFamilyId equals b.Id
                            where a.Id == idHead
        select new ResidentFamilyModel
        {
            Id = a.Id, 
            Name = a.Name,
            Gender = a.Gender,
            Age = a.Age,
            Relationship = a.Relationship,
            HeadOfFamilyId = a.HeadOfFamilyId,
            HeadOfFamilyName = b.Name
        }).FirstOrDefaultAsync();
        
        return result; 
    }

    public async Task<ResidentFamilyModel> FamilyEdit(ResidentFamilyModel model, Guid idCurrentUser)
    {
        var dataEdited = await Db.ResidentFamilies.FirstOrDefaultAsync(x => x.Id == model.Id);
        if (model == null)
        {
            throw new InvalidOperationException($"User dengan ID {model.Id} tidak dapat ditemukan");
        }

        dataEdited.Name = model.Name;
        dataEdited.Gender = model.Gender;
        dataEdited.Age = model.Age;
        dataEdited.Relationship = model.Relationship;
        dataEdited.UpdatedBy = idCurrentUser;
        dataEdited.UpdatedDate = DateTime.Now;
        dataEdited.AuditActivty = AuditActivtyType.UPDATE;

        Db.Update(dataEdited);
        await Db.SaveChangesAsync();


        return model;
    }

    public async Task<ResidentFamily> Get(Guid id)
    {
        var family = await Db.ResidentFamilies.FirstOrDefaultAsync(x => x.Id == id);
        return family;
    }

    public async Task<List<ResidentFamilyModel>> GetId(Guid idHead)
    {
        var result = await 
        (from a in Db.ResidentFamilies
        join b in Db.Residents on a.HeadOfFamilyId equals b.Id 
        where a.HeadOfFamilyId == idHead
        select new ResidentFamilyModel
        {
            Id = a.Id, 
            Name = a.Name,
            Gender = a.Gender,
            Age  = a.Age,
            Relationship = a.Relationship,
            HeadOfFamilyId = a.HeadOfFamilyId,
            HeadOfFamilyName = b.Name
        }).ToListAsync();

        return result;
    }

    
}
