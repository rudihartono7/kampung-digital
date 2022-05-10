using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;
public class ProductService : BaseDbService, IProductService
{
    public ProductService(KampDigiContext dbContext) : base(dbContext)
    {
    }

    public async Task<Product> Add(Product obj)
    {
        if (await Db.Products.AnyAsync(x => x.Id == obj.Id))
        {
            throw new InvalidOperationException($"Product with ID {obj.Id} is already exist");
        }

        obj.AuditActivty = Trisatech.KampDigi.Domain.Entities.AuditActivtyType.INSERT;
        obj.CreatedBy = Guid.NewGuid();
        obj.UpdatedBy = obj.CreatedBy;
        obj.CreatedDate = DateTime.Now;
        await Db.AddAsync(obj);
        await Db.SaveChangesAsync();

        return obj;
    }

  

    public async Task<bool> Delete(Guid id)
    {
        var product = await Db.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with ID {id} doesn't exist");
        }
        Db.Remove(product);
        await Db.SaveChangesAsync();

        return true;
    }

    async Task<ProductModel> IProductService.DetailProduk(Guid idCurrentUser)
    {
        var result = await (from a in Db.Products 
                            join b in Db.Residents on a.CreatedBy equals b.Id
                            where a.Id == idCurrentUser
        select new ProductModel
        {
            Id = a.Id, 
            Name = a.Name,
            Price = a.Price,
            PublicLink = a.PublicLink,
            SellerName = a.SellerName,
            WhatsappNumber = a.WhatsappNumber,
            ImageUrl = a.ImageUrl,
            Desc = a.Desc,
            IdAkun = a.CreatedBy,
            NamaAkun = b.Name,
        }).FirstOrDefaultAsync();
        
        return result; 
    }

    public async Task<ProductModel> EditProduk(ProductModel request, Guid idCurrentUser)
    {
        var edit = await Db.Products.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (request == null)
            {
                throw new InvalidOperationException($"User dengan ID {request.Id} tidak dapat ditemukan");
            }

        edit.Name = request.Name;
        edit.Price = request.Price;
        edit.PublicLink = request.PublicLink;
        edit.SellerName = request.SellerName;
        edit.WhatsappNumber = request.WhatsappNumber;
        if (!string.IsNullOrEmpty(request.ImageUrl))
            {
                edit.ImageUrl = request.ImageUrl;
            }
        edit.Desc = request.Desc;
        edit.UpdatedBy = idCurrentUser;
        edit.UpdatedDate = DateTime.Now;
        edit.AuditActivty = Trisatech.KampDigi.Domain.Entities.AuditActivtyType.UPDATE;

        Db.Update(edit);
        await Db.SaveChangesAsync();

        return request;

    }

    public Task<List<Product>> Get(int limit, int offset, string keyword)
    {
        throw new NotImplementedException();
    }

    public async Task<Product> Get(Guid id)
    {
        var product = await Db.Products.FirstOrDefaultAsync(x => x.Id == id);
        return product;
    }

    public Task<Product> Get(Expression<Func<Product, bool>> func)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Product>> GetAll()
    {
        return await Db.Products.ToListAsync();
    }

    public async Task<List<ProductModel>> GetId(Guid idCurrentUser)
    {
        var result = await (from a in Db.Products where a.CreatedBy == idCurrentUser
        select new ProductModel
        {
            Id = a.Id, 
            Name = a.Name,
            Price = a.Price,
            PublicLink = a.PublicLink,
            SellerName = a.SellerName,
            WhatsappNumber = a.WhatsappNumber,
            ImageUrl = a.ImageUrl,
            Desc = a.Desc
        }).ToListAsync();

        return result;
    }

    public async Task<Product> Update(Product obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException("Id cannot be null");
        }

        var product = await Db.Products.FirstOrDefaultAsync(x => x.Id == obj.Id);

        if (product == null)
        {
            throw new InvalidOperationException($"Produk with ID {obj.Id} doesn't exist in database");
        }

        product.Name = obj.Name;
        product.Price = obj.Price;
        product.PublicLink = obj.PublicLink;
        product.SellerName = obj.SellerName;
        product.WhatsappNumber = obj.WhatsappNumber;
        if (!string.IsNullOrEmpty(obj.ImageUrl))
            {
                product.ImageUrl = obj.ImageUrl;
            }
        product.Desc = obj.Desc;
        product.AuditActivty = Trisatech.KampDigi.Domain.Entities.AuditActivtyType.UPDATE;

        Db.Update(product);
        await Db.SaveChangesAsync();

        return product;
    }

    async Task<ProductModel> IProductService.AddProduk(ProductModel request, Guid idCurrentUser)
    {
         if (await Db.Products.AnyAsync(x => x.Id == request.Id))
        {
            throw new InvalidOperationException($"Product with ID {request.Id} is already exist");
        }

        var newProduct = new Product{
            Name = request.Name,
            Price = request.Price,
            PublicLink = request.PublicLink,
            SellerName = request.SellerName,
            WhatsappNumber = request.WhatsappNumber,
            ImageUrl = request.ImageUrl,
            Desc = request.Desc,
            CreatedBy = idCurrentUser,
            CreatedDate = DateTime.Now,
            AuditActivty = Trisatech.KampDigi.Domain.Entities.AuditActivtyType.INSERT,
        };

          await Db.Products.AddAsync(newProduct);
          await Db.SaveChangesAsync();

          return request;
    }
}
