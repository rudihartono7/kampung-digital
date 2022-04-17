using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;
public class CommentService : BaseDbService, ICommentService {
    public CommentService(KampDigiContext dbContext) : base(dbContext)
    {
    }

    public async Task<Comment> Add(Comment obj)
    {
         if (await Db.Comments.AnyAsync(x => x.Id == obj.Id))
          {
               throw new InvalidOperationException($"Comment with ID {obj.Id} is already exist");
          }

          await Db.AddAsync(obj);
          await Db.SaveChangesAsync();

          return obj;
    }

    public async Task<bool> Delete(Guid id)
    {
        var Comment = await Db.Comments.FirstOrDefaultAsync(x => x.Id == id);

          if (Comment == null)
          {
               throw new InvalidOperationException($"Comment with ID {id} doesn't exist");
          }

          Db.Comments.RemoveRange(Db.Comments.Where(x => x.Id == id));
          Db.Remove(Comment);
          await Db.SaveChangesAsync();

          return true;
    }

    public async Task<List<Comment>> Get(int limit, int offset, string keyword)
    {
        if (string.IsNullOrEmpty(keyword))
          {
               keyword = "";
          }

          return await Db.Comments.Skip(offset).Take(limit).ToListAsync();
    }

    public async Task<Comment> Get(Guid id)
    {
        var result = await Db.Comments.FirstOrDefaultAsync(x => x.Id == id);
          if (result == null)
          {
               throw new InvalidOperationException($"Comment with ID {id} doesn't exist");
          }
          return result;
    }

    public Task<Comment> Get(Expression<Func<Comment, bool>> func)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Comment>> GetAll(Guid PostId)
    {
        return await Db.Comments.Where(x => x.PostId == PostId).ToListAsync();
    }

    public async Task<List<Comment>> GetAll()
    {
        return await Db.Comments.ToListAsync();
    }

    public Task<List<CommentModel>> GetComment(int Id)
    {
        throw new NotImplementedException();
    }

    async Task<List<CommentModel>> GetComment(Guid PostId)
    {
        var result = await (from a in Db.Comments
                              where a.PostId == PostId
                              select new CommentModel
                              {
                                    Id = a.Id,
                                    Desc = a.Desc,
                                    PostId = a.PostId
                                }).ToListAsync();
          return result;
    }

    public async Task<Comment> Update(Comment obj)
    {
        if (obj == null)
          {
               throw new ArgumentException("Comment cannot be null");
          }

          var result = await Db.Comments.FirstOrDefaultAsync(x => x.Id == obj.Id);

          if (result == null)
          {
               throw new InvalidOperationException($"Comment with ID{obj.Id} doesn't exist in database");
          }
            result.Id = obj.Id;
            result.Desc = obj.Desc;
            result.PostId = obj.PostId;

          Db.Update(result);
          await Db.SaveChangesAsync();

          return result;
    }

    Task<List<CommentModel>> ICommentService.Get(Guid Id)
    {
        throw new NotImplementedException();
    }
}
