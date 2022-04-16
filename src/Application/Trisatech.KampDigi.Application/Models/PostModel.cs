using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Models
{
    public class PostModel
    {

        public Guid Id { get; set; }
        public string PostSubject { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Image { get; set; }
        public PostType Type { get; set; }
        public bool IsResidentProgram { get; set; }
    public Post ConvertToDbModel(){
            return new Post {
                PostSubject = this.PostSubject,
                Id = this.Id,
                Title = this.Title,
                Desc = this.Desc,
                Image = this.Image,
                Type = this.Type,
                IsResidentProgram = this.IsResidentProgram
            };
    }
    }
}