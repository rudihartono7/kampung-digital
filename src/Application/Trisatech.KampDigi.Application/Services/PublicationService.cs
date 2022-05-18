using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Domain.Entities;
using System.Linq.Expressions;

namespace Trisatech.KampDigi.Application.Interfaces;
public class PublicationService : BaseDbService, IPublicationService {
    public PublicationService(KampDigiContext dbContext) : base(dbContext)
    {
    }

    public async Task<Publication> Add(Publication obj)
    {
        if(await Db.Publications.AnyAsync(x=>x.Id== obj.Id)){
            throw new InvalidOperationException($"Alamat with ID {obj.Id} is already exist");
        }

        await Db.AddAsync(obj);
        await Db.SaveChangesAsync();

        return obj;
    }

    public Task<bool> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Delete(Guid id)
    {
        var publication = await Db.Publications.FirstOrDefaultAsync(x=>x.Id == id);

        if(publication == null) {
            throw new InvalidOperationException($"Kategori with ID {id} doesn't exist");
        }

        Db.Publications.RemoveRange(Db.Publications.Where(x=>x.Id == id));
        
        Db.Remove(publication);
        await Db.SaveChangesAsync();

        return true;
    }

    public Task<List<Publication>> Get(int limit, int offset, string keyword)
    {
        throw new NotImplementedException();
    }

    public Task<Publication> Get(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Publication> Get(Expression<Func<Publication, bool>> func)
    {
        throw new NotImplementedException();
    }

    public async Task<Publication> Get(Guid id)
    {
        var result = await Db.Publications.FirstOrDefaultAsync(x=>x.Id == id);

        if(result == null)
        {
            throw new InvalidOperationException($"publication with ID {id} doesn't exist");
        }

        return result;
    }

    public async Task<List<Publication>> GetAll()
    {
         return await Db.Publications.ToListAsync();
    }

    public async Task<Publication> Update(Publication obj)
    {
        if(obj == null)
        {
            throw new ArgumentNullException("publication cannot be null");
        }

        var publication = await Db.Publications.FirstOrDefaultAsync(x=>x.Id == obj.Id);

        if(publication == null) {
            throw new InvalidOperationException($"publication with ID {obj.Id} doesn't exist in database");
        }

        publication.Id = obj.Id;
        publication.Title = obj.Title;
        publication.PublishDate = obj.PublishDate;
        publication.Slag = obj.Slag;
        publication.Content = obj.Content;
        publication.ImageLink = obj.ImageLink;
        publication.Writer = obj.Writer;
        publication.Source = obj.Source;
                
        Db.Update(publication);
        await Db.SaveChangesAsync();

        return publication;
    }
    
}
