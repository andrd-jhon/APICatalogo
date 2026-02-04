using System.Linq.Expressions;

namespace APICatalogo.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> Query();
        Task<T?> Get(Expression<Func<T, bool>> predicate);
        T Create(T entity);
        Task<T> Update(T entity);
        T Delete(int id);
    }
}
