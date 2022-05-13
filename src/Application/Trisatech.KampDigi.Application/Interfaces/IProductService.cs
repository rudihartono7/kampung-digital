
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain.Entities;


namespace Trisatech.KampDigi.Application.Interfaces;
public interface IProductService : ICrudService<Product>{

    Task<List<ProductModel>> GetId(Guid idCurrentUser);
     Task<ProductModel> AddProduk (ProductModel request, Guid idCurrentUser);
     Task<ProductModel> EditProduk (ProductModel request, Guid idCurrentUser);
     Task<ProductModel> DetailProduk (Guid idCurrentUser);

}
