using Trisatech.KampDigi.Application.Models;

namespace Trisatech.KampDigi.Application.Interfaces;
public interface IResidentFundService {
    Task<ResidentFundModel> GetCurrentBalance(int? year = null);
}
