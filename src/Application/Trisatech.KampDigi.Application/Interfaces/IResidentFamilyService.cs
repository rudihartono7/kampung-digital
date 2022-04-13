using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;
public interface IResidentFamilyService : ICrudService<ResidentFamily>{

    Task<List<ResidentFamilyModel>> GetId(Guid IdHead);
}
