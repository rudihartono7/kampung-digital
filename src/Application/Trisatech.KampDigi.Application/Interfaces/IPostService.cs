using Trisatech.KampDigi.Application.Models;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Interfaces;
public interface IPostService : ICrudService<Post>
{
    //mengambil data berdasarkan id username pada tabel users
    Task<Post> Get(Guid id);
    
}
