using System.Data;
using System.Reflection.Metadata;
using System.Data.Common;
using System.ComponentModel;
using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Application.Models.Bill;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;
public class ResidentBillService : BaseDbService, IResidentBillService
{
    public ResidentBillService(KampDigiContext dbContext) : base(dbContext)
    {  
    }
    
    public async Task<ResidentBill> Add(ResidentBill obj)
    {
        if(await Db.ResidentBills.AnyAsync(x=>x.Id== obj.Id))
        {
            throw new Exception("Resident Bill Base Info already exists");
        }

        obj.CreatedDate = DateTime.Now;
        await Db.ResidentBills.AddAsync(obj);
        await Db.SaveChangesAsync();

        return obj;
    }

    public async Task<bool> Delete(Guid id)
    {
        var bill = await Db.ResidentBills.FirstOrDefaultAsync(x => x.Id == id);

        if(bill == null)
        {
            throw new Exception("Resident Bill Base Info not found (DELETE)");
        }

        Db.ResidentBills.Remove(bill);
        await Db.SaveChangesAsync();

        return true;
    }

    public Task<List<ResidentBill>> Get(int limit, int offset, string keyword)
    {
        throw new NotImplementedException();
    }

    public Task<ResidentBill> Get(Guid id)
    {
        var result = Db.ResidentBills.FirstOrDefaultAsync(x => x.Id == id);

        if(result == null)
        {
            throw new Exception("Resident Bill Base Info not found (GET)");
        }

        return result;
    }

    public Task<ResidentBill> Get(Expression<Func<ResidentBill, bool>> func)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ResidentBill>> GetAll()
    {
        return await Db.ResidentBills.ToListAsync();
    }

    public async Task<ResidentBill> Update(ResidentBill obj)
    {
        if(obj == null)
        {
            throw new Exception("Resident Bill Base Info not found (Update)");
        }

        var bill = await Db.ResidentBills.FirstOrDefaultAsync(x => x.Id == obj.Id);

        if(bill == null)
        {
            throw new Exception("Resident Bill Base Info not found");
        }
        
        bill.Evidence = obj.Evidence;
        bill.Status = obj.Status;
        bill.UpdatedDate = DateTime.Now;

        Db.Update(bill);
        await Db.SaveChangesAsync();

        return bill;
    }

    public async Task<List<ResidentBillModel>> GetResident(Guid residentId)
    {
        var result = await (from a in Db.ResidentBills
                            join b in Db.Residents on a.ResidentTo equals b.Id
                            where a.ResidentTo == residentId
                            select new ResidentBillModel
                            {
                                Id = a.Id,
                                ResidentTo = a.ResidentTo,
                                Nominal = a.Nominal,
                                NameResident = b.Name,
                                Status = a.Status,
                            }).ToListAsync();

        return result;
    }
}