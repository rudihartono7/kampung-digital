namespace Trisatech.KampDigi.Application.Interfaces;
public class BaseDbService {
    protected readonly KampDigi.Domain.KampDigiContext Db;
    public BaseDbService(KampDigi.Domain.KampDigiContext dbContext)
    {
        Db = dbContext;
    }
}
