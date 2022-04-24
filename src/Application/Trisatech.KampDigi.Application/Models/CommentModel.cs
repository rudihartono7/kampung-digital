using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Models
{
    public class CommentModel
    {   
        public Guid Id { get; set; }
        public string Desc { get; set; }
        public Guid PostId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        
        public Comment ConvertToDbModelCreate(){
            return new Comment {
                Desc = this.Desc,
                PostId = this.PostId
            };
        }
        public Comment ConvertToDbModelEdit(){
            return new Comment {
                Id = this.Id,
                Desc = this.Desc,
                PostId = this.PostId
            };
        }
    }
}
