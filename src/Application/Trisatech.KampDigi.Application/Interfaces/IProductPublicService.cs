using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain.Entities;


namespace Trisatech.KampDigi.Application.Interfaces;
public interface IProductPublicService{
Task<Product> GetProduct(Guid id);
Task<List<Product>> GetAllProduct();

   

}