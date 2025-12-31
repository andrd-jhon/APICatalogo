using System.Linq.Expressions;

namespace APICatalogo.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> Query();
        T? Get(Expression<Func<T, bool>> predicate);
        T Create(T entity);
        T Update(T entity);
        T Delete(int id);
    }
}
