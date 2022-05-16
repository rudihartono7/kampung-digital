using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Domain.Entities;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models;

namespace Trisatech.KampDigi.Application.Interfaces;
public class PostService : BaseDbService, IPostService
{
    public PostService(KampDigiContext dbContext) : base(dbContext)
    {
    }

    public async Task<Post> Add(Post obj)
    {
         throw new NotImplementedException();
    }

    public async Task<Post> Add(Post obj, Guid Id)
    {
        obj.Title = obj.Title;
        obj.Desc = obj.Desc;
        obj.Image = obj.Image;
        obj.Type = obj.Type;
        obj.AuditActivty = Trisatech.KampDigi.Domain.Entities.AuditActivtyType.INSERT;
        obj.CreatedBy = Id;
        obj.UpdatedBy = obj.CreatedBy;
        obj.CreatedDate = DateTime.Now;
        await Db.AddAsync(obj);
        await Db.SaveChangesAsync();

        return obj;
    }

    public async Task<bool> Delete(Guid id)
    {
        var post = await Db.Posts.FirstOrDefaultAsync(x => x.Id == id);

        if (post == null)
        {
            throw new InvalidOperationException($"Post with ID {id} doesn't exist");
        }
        Db.Posts.RemoveRange(Db.Posts.Where(x => x.Id == id));
        Db.Remove(post);
        await Db.SaveChangesAsync();

        return true;
    }

    public async Task<List<Post>> Get(int limit, int offset, string keyword)
    {
        if (string.IsNullOrEmpty(keyword))
        {
            keyword = "";
        }

        return await Db.Posts
        .Skip(offset)
        .Take(limit).ToListAsync();
    }

    public async Task<PostModel> Get(Guid id)
    {
        var result = await (from p in Db.Posts
                            join u in Db.Users on p.CreatedBy equals u.Id
                            select new PostModel
                            {
                                Id = p.Id,
                                PostSubject = p.PostSubject,
                                Title = p.Title,
                                Desc = p.Desc,
                                Image = p.Image,
                                Type = p.Type,
                                IsResidentProgram = p.IsResidentProgram,
                                CreatedDate = p.CreatedDate,
                                UpdatedDate = p.UpdatedDate,
                                Name = u.Name,
                            }).FirstOrDefaultAsync();      
        // var result = await Db.Posts.FirstOrDefaultAsync(x => x.Id == id);
        // result = await Db.Users.FirstOrDefaultAsync(x => x.Id == result.CreatedBy);
        if (result == null)
        {
            throw new InvalidOperationException($"Post with ID {id} doesn't exist");
        }

        return result;
    }

    public Task<Post> Get(Expression<Func<Post, bool>> func)
    {
        throw new NotImplementedException();
    }

    public Task Get(int value)
    {
        throw new NotImplementedException();
    }

    public async Task<List<PostModel>> GetAllPost()
    {
        var post = await (from p in Db.Posts
                            join u in Db.Users on p.CreatedBy equals u.Id into tempName
                            from u in tempName.DefaultIfEmpty()
                            select new PostModel
                            {
                                Id = p.Id,
                                PostSubject = p.PostSubject,
                                Title = p.Title,
                                Desc = p.Desc,
                                Image = p.Image,
                                Type = p.Type,
                                IsResidentProgram = p.IsResidentProgram,
                                CreatedDate = p.CreatedDate,
                                UpdatedDate = p.UpdatedDate,
                                Name = u.Name,
                            }).ToListAsync();
        return post;
    }

    public Task<List<PostModel>> GetAll(Guid PostId)
    {
        throw new NotImplementedException();
    }



    public async Task<Post> Update(Post obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException("Post with ID {id} doesn't exist");
        }

        var post = await Db.Posts.FirstOrDefaultAsync(x => x.Id == obj.Id);

        if (post == null)
        {
            throw new InvalidOperationException($"");
        }

        post.Id = obj.Id;
        post.PostSubject = obj.PostSubject;
        post.Title = obj.Title;
        post.Desc = obj.Desc;
        if (!string.IsNullOrEmpty(obj.Image))
        {
            post.Image = obj.Image;
        }
        post.Type = obj.Type;
        post.IsResidentProgram = obj.IsResidentProgram;
        post.AuditActivty = Trisatech.KampDigi.Domain.Entities.AuditActivtyType.UPDATE;
        post.UpdatedDate = DateTime.Now;
        Db.Update(post);
        await Db.SaveChangesAsync();

        return post;
    }

    Task<List<Post>> ICrudService<Post>.GetAll()
    {
        throw new NotImplementedException();
    }

    Task<Post> ICrudService<Post>.Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<PostModel>> GetPostLanding(){
        var post = await (from p in Db.Posts
                            join u in Db.Users on p.CreatedBy equals u.Id into tempName
                            from u in tempName.DefaultIfEmpty()
                            select new PostModel
                            {
                                Id = p.Id,
                                PostSubject = p.PostSubject,
                                Title = p.Title,
                                Desc = p.Desc,
                                Image = p.Image,
                                Type = p.Type,
                                IsResidentProgram = p.IsResidentProgram,
                                CreatedDate = p.CreatedDate,
                                UpdatedDate = p.UpdatedDate,
                                Name = u.Name,
                            }).Take(5).ToListAsync();
        return post;
    }
}