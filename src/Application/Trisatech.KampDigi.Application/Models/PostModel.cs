using Microsoft.AspNetCore.Http;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Models
{
    public class PostModel
    {

        public Guid Id { get; set; }
        public string PostSubject { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string? Image { get; set; }
        public string ImageSrc {
            get {
                return (string.IsNullOrEmpty(Image) ? "~/images/no-image.png" : Image);
            }
        }
        public IFormFile? ImageFile { get; set; }
        public PostType Type { get; set; }
        public bool IsResidentProgram { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Name { get; set; }
    public Post ConvertToDbModelAdd(){
            return new Post {
                PostSubject = this.PostSubject,
                Title = this.Title,
                Desc = this.Desc,
                Image = this.Image,
                Type = this.Type,
                IsResidentProgram = this.IsResidentProgram,
            };
    }
    public Post ConvertToDbModelEdit(){
            return new Post {
                Id = this.Id,
                PostSubject = this.PostSubject,
                Title = this.Title,
                Desc = this.Desc,
                Image = this.Image,
                Type = this.Type,
                IsResidentProgram = this.IsResidentProgram,
            };
    }
    }
}