using Trisatech.KampDigi.Application.Models.Resident;
using Trisatech.KampDigi.Application.Models.ResidentFamilies;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;
public interface IResidentFamilyService
{
    Task<ResidentFamily> Get(Guid id);
    Task<List<ResidentFamilyModel>> GetId(Guid idHead);
    Task<ResidentFamilyModel> FamilyAdd(ResidentFamilyModel model, Guid idCurrentUser);
    Task<ResidentFamilyModel> FamilyDetail(Guid idHead);
    Task<ResidentFamilyModel> FamilyEdit(ResidentFamilyModel model, Guid idCurrentUser);
    Task<bool> FamilyDelete(Guid id);
}
