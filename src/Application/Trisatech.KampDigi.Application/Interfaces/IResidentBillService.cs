using System.Collections.Generic;
using System.CodeDom.Compiler;
using Trisatech.KampDigi.Domain.Entities;
using Trisatech.KampDigi.Application.Models;

namespace Trisatech.KampDigi.Application.Interfaces;
public interface IResidentBillService : ICrudService<ResidentBill>
{
    Task<List<ResidentBillModel>> GetResident(Guid residentId);
}