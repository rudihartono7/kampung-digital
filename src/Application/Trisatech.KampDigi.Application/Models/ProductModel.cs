using Trisatech.KampDigi.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Trisatech.KampDigi.Application.Models;
public class ProductModel
{

    public ProductModel()
    {

    }

    public ProductModel(Product item)
    {
        Id = item.Id;
        Name = item.Name;
        Price = this.Price;
        PublicLink = this.PublicLink;
        SellerName = this.SellerName;
        WhatsappNumber = this.WhatsappNumber;
        ImageUrl = this.ImageUrl;
        Desc = this.Desc;

    }
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string PublicLink { get; set; }
    public string SellerName { get; set; }
    public string WhatsappNumber { get; set; }
    public string ImageUrl { get; set; }
    public string NamaAkun {get; set; }
    public Guid IdAkun {get; set; }


    public string Imagesrc
    {
        get
        {
            return (string.IsNullOrEmpty(ImageUrl) ? "images/default.png" : ImageUrl);
        }
    }

    public IFormFile? ImageFile {get;set; }

    public string Desc { get; set; }

    public Product ConvertToDbModel()
    {
        return new Product
        {
            Id = (this.Id == null) ? Guid.NewGuid() : this.Id.Value,
            Name = this.Name,
            Price = this.Price,
            PublicLink = this.PublicLink,
            SellerName = this.SellerName,
            WhatsappNumber = this.WhatsappNumber,
            ImageUrl = this.ImageUrl,
            Desc = this.Desc
        };
    }
}
