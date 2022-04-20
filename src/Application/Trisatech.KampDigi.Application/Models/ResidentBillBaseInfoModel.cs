using System;
using System.Globalization;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Models;
public class ResidentBillBaseInfoModel
{
    public ResidentBillBaseInfoModel()
    {
        
    }
    public ResidentBillBaseInfoModel(ResidentBillBaseInfo obj)
    {
        Id = obj.Id;
        Year = obj.Year;
        Nominal = obj.Nominal;
        MontlyBillOpenDate = obj.MontlyBillOpenDate;
        DueDateNumber = obj.DueDateNumber;
    }
    public Guid Id { get; set; }
    public int Year { get; set; }
    public decimal Nominal { get; set; }
    public DateTime MontlyBillOpenDate { get; set; }
    public int DueDateNumber { get; set; }

    public ResidentBillBaseInfo ConvertToDbModel()
    {
        return new ResidentBillBaseInfo
        {
            Year = this.Year,
            Nominal = this.Nominal,
            MontlyBillOpenDate = this.MontlyBillOpenDate,
            DueDateNumber = this.DueDateNumber
        };
    }

    public ResidentBillBaseInfo ConvertUpdateToDbModel()
    {
        return new ResidentBillBaseInfo
        {
            Id = this.Id,
            Year = this.Year,
            Nominal = this.Nominal,
            MontlyBillOpenDate = this.MontlyBillOpenDate,
            DueDateNumber = this.DueDateNumber
        };
    }
}