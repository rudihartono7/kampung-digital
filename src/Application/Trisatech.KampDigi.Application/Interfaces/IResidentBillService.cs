using System.Collections.Generic;
using System.CodeDom.Compiler;
using Trisatech.KampDigi.Domain.Entities;
using Trisatech.KampDigi.Application.Models;

namespace Trisatech.KampDigi.Application.Interfaces;
public interface IResidentBillService 
{
    Task<ResidentBill> Generated(ResidentBill newResidentBill);
    Task<List<ResidentBillModel>> GetAll();
    Task<List<ResidentBillModel>> Get(Guid idResident);
    //Task<ResidentBillModel> GetDetail(Guid idResidentBill, Guid idResident);
}