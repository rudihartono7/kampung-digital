using Trisatech.KampDigi.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Trisatech.KampDigi.Application.Models;

public class PublicationModel {

    public Guid Id { get; set; }
    
    [Required]
    public string Title { get; set; }
        
    public string Slag { get; set; }
        
    public DateTime PublishDate { get; set; }
    public string ImageLink { get; set; }


    public IFormFile? ImageFile { get; set; }
        
    [Required]
    public string Content { get; set; }        
    [Required]
    public string Writer { get; set; }
    public string Source { get; set; }

    public PublicationModel(){
        Title = string.Empty;
        // PublishDate = PublishDate ;
        Writer = string.Empty;
        Content = string.Empty;
    }

    public PublicationModel(Publication item){
        Id = item.Id;
        Title = item.Title;
        Slag = item.Slag;
        PublishDate = item.PublishDate;
        ImageLink = item.ImageLink;
        Content = item.Content;
        Writer = item.Writer;
        Source = item.Source;
    }

    public Publication ConvertToDbModel(){
        return new Publication{
            
            Id = (this.Id == Guid.Empty) ? Guid.NewGuid() : this.Id,
            Title = this.Title,
            Slag = this.Slag,
            PublishDate = DateTime.Now,
            ImageLink = this.ImageLink,
            Content = this.Content,
            Writer = this.Writer,
            Source = this.Source,
        };
    }

}