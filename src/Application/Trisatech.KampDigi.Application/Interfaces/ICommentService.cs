using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;
public interface ICommentService: ICrudService<Comment> {
    Task<List<CommentModel>> GetComments(Guid Id);
}
