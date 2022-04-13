using System.Linq.Expressions;
namespace Trisatech.KampDigi.Application.Interfaces;

    public interface ICrudService<T> where T : class
    {
        Task<T> Add (T obj);    
        Task<T> Update (T obj); 
        Task<bool> Delete (Guid id);
        Task<List<T>> GetAll();
        Task<List<T>> Get (int limit, int offset, string keyword);
        Task<T?> Get(Guid id);

        Task<T?> Get(Expression<Func<T, bool>> func);

    }
