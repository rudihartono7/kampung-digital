using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Domain.Entities;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models;

namespace Trisatech.KampDigi.Application.Interfaces;
public class PostService : BaseDbService, IPostService {
    public PostService(KampDigiContext dbContext) : base(dbContext)
    {
    }

    public async Task<Post> Add(Post obj)
    {
        if(await Db.Posts.AnyAsync(x=>x.Id == obj.Id)){
            throw new InvalidOperationException($"Post with Id {obj.Id} already exists");
        }

        await Db.AddAsync(obj);
        await Db.SaveChangesAsync();
        
       return obj;
    }

    public async Task<bool> Delete(Guid id)
    {
        var post = await Db.Posts.FirstOrDefaultAsync(x=>x.Id == id);

        if(post == null) {
            throw new InvalidOperationException($"Post with ID {id} doesn't exist");
        }
        Db.Posts.RemoveRange(Db.Posts.Where(x=>x.Id == id));
        Db.Remove(post);
        await Db.SaveChangesAsync();

        return true;
    }

    public async Task<List<Post>> Get(int limit, int offset, string keyword)
    {
        if(string.IsNullOrEmpty(keyword)){
            keyword = "";
        }

        return await Db.Posts
        .Skip(offset)
        .Take(limit).ToListAsync();
    }

    public async Task<Post> Get(Guid id)
    {
        var result = await Db.Posts.FirstOrDefaultAsync(x => x.Id == id);

        if(result == null)
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

    public async Task<List<Post>> GetAll()
    {
        return await Db.Posts.ToListAsync();
    }

    public Task<List<PostModel>> GetAll(Guid PostId)
    {
        throw new NotImplementedException();
    }

    public async Task<Post> Update(Post obj)
    {
        if(obj == null)
        {
            throw new ArgumentNullException("Post with ID {id} doesn't exist");
        }

        var post = await Db.Posts.FirstOrDefaultAsync(x=>x.Id == obj.Id);

        if(post == null) {
            throw new InvalidOperationException($"");
        }

        post.Id = obj.Id;
        post.PostSubject = obj.PostSubject;
        post.Title = obj.Title;
        post.Desc = obj.Desc;
        post.Image = obj.Image;
        post.Type = obj.Type;
        
        Db.Update(post);
        await Db.SaveChangesAsync();

        return post;
    }
}