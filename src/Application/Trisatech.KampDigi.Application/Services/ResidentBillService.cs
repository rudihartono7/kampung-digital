using System.Data.Common;
using System.ComponentModel;
using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;
public class ResidentBillService : BaseDbService, IResidentBillService
{
    public ResidentBillService(KampDigiContext dbContext) : base(dbContext)
    {  
    }

    public async Task<ResidentBill> Generated(ResidentBill newResidentBill)
    {
        await Db.AddAsync(newResidentBill);
        await Db.SaveChangesAsync();

        return newResidentBill;
    }

    public async Task<List<ResidentBillModel>> Get(Guid idResident)
    {
        var result = await (from a in Db.ResidentBills
        join b in Db.Residents on a.ResidentTo equals b.Id
        where a.ResidentTo == idResident
        select new ResidentBillModel
        {
            Id = a.Id,
            ResidentTo = a.ResidentTo,
            NameResident = b.Name,
            Year = a.Year,
            Month = a.Month,
            Nominal = a.Nominal,
            PaymentDate = a.PaymentDate,
            DueDate = a.DueDate,
            BillOpenDate = a.BillOpenDate,
            Evidence = a.Evidence,
            Note = a.Note,
            Status = a.Status
        }).ToListAsync();
        
        return result;
    }

    public async Task<List<ResidentBillModel>> GetAll()
    {
        var result = await (from a in Db.ResidentBills
        join b in Db.Residents on a.ResidentTo equals b.Id
        select new ResidentBillModel
        {
            Id = a.Id,
            ResidentTo = a.ResidentTo,
            NameResident = b.Name,
            Year = a.Year,
            Month = a.Month,
            Nominal = a.Nominal,
            PaymentDate = a.PaymentDate,
            DueDate = a.DueDate,
            BillOpenDate = a.BillOpenDate,
            Evidence = a.Evidence,
            Note = a.Note,
            Status = a.Status
        }).ToListAsync();
        
        return result;
    }

    
}