using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Models
{
    public class CommentModel
    {   
        public Guid Id { get; set; }
        public string Desc { get; set; }
        public Guid PostId { get; set; }
        
        public Comment ConvertToDbModel(){
            return new Comment {
                Desc = this.Desc,
                PostId = this.PostId
            };
    }
    }
}
