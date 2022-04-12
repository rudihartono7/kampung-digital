using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain;

namespace Trisatech.KampDigi.Application.Interfaces;
public class ResidentFundService : BaseDbService, IResidentFundService {
    public ResidentFundService(KampDigiContext dbContext) : base(dbContext)
    {
    }

    async Task<ResidentFundModel> IResidentFundService.GetCurrentBalance(int? year = null)
    {
        if(year == null)
        {
            year = DateTime.Now.Year;
        }

        return await (from a in Db.ResidentFunds
        join b in Db.Users on a.UpdatedBy equals b.Id
        where a.Year == year.Value
        select new ResidentFundModel
        {
            Year = a.Year,
            BeginingBalance = a.BeginingBalance,
            CurrentBalance = a.CurrentBalance,
            EndingBalance = a.EndingBalance,
            HoldBalance = a.HoldBalance,
            LastUpdate = a.UpdatedDate,
            UpdatedBy = b.Name,
        }).FirstOrDefaultAsync();
    }
}
