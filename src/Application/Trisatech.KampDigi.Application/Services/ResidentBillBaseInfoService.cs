using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;
public class ResidentBillBaseInfoService : BaseDbService, IResidentBillBaseInfoService
{
    public ResidentBillBaseInfoService(KampDigiContext dbContext) : base(dbContext)
    {
    }

    public async Task<ResidentBillBaseInfo> Add(ResidentBillBaseInfo obj)
    {
        if(await Db.ResidentBillBaseInfos.AnyAsync(x=>x.Id== obj.Id))
        {
            throw new Exception("Resident Bill Base Info already exists");
        }

        await Db.ResidentBillBaseInfos.AddAsync(obj);
        await Db.SaveChangesAsync();

        return obj;
    }

    public async Task<bool> Delete(Guid id)
    {
        var bill = await Db.ResidentBillBaseInfos.FirstOrDefaultAsync(x => x.Id == id);

        if(bill == null)
        {
            throw new Exception("Resident Bill Base Info not found");
        }

        Db.ResidentBillBaseInfos.Remove(bill);
        await Db.SaveChangesAsync();

        return true;
    }

    public Task<List<ResidentBillBaseInfo>> Get(int limit, int offset, string keyword)
    {
        throw new NotImplementedException();
    }

    public Task<ResidentBillBaseInfo> Get(Guid id)
    {
        var result = Db.ResidentBillBaseInfos.FirstOrDefaultAsync(x => x.Id == id);

        if(result == null)
        {
            throw new Exception("Resident Bill Base Info not found");
        }

        return result;
    }

    public Task<ResidentBillBaseInfo> Get(Expression<Func<ResidentBillBaseInfo, bool>> func)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ResidentBillBaseInfo>> GetAll()
    {
        return await Db.ResidentBillBaseInfos.ToListAsync();
    }

    public async Task<ResidentBillBaseInfo> Update(ResidentBillBaseInfo obj)
    {
        if(obj == null)
        {
            throw new Exception("Resident Bill Base Info not found");
        }

        var bill = await Db.ResidentBillBaseInfos.FirstOrDefaultAsync(x => x.Id == obj.Id);

        if(bill == null)
        {
            throw new Exception("Resident Bill Base Info not found");
        }

        bill.Year = obj.Year;
        bill.Nominal = obj.Nominal;
        bill.MontlyBillOpenDate = obj.MontlyBillOpenDate;
        bill.DueDateNumber = obj.DueDateNumber;

        Db.Update(bill);
        await Db.SaveChangesAsync();

        return bill;
    }
}
