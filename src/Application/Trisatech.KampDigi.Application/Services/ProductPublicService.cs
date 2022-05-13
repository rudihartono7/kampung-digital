using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;
public class ProductPublicService : BaseDbService, IProductPublicService
{
    public ProductPublicService(KampDigiContext dbContext) : base(dbContext)
    {
    }

    
    public async Task<Product> GetProduct(Guid id)
    {
        var product = await Db.Products.FirstOrDefaultAsync(x => x.Id == id);
        return product;
    }

    

    public async Task<List<Product>> GetAllProduct()
    {
        return await Db.Products.ToListAsync();
    }

    
}