using System.Linq.Expressions;

namespace APICatalogo.Interfaces
{
    public interface IRepository<T>
    {
        IReadOnlyList<T> GetAll();
        IQueryable<T> Query();
        T? Get(Expression<Func<T, bool>> predicate);
        T Create(T entity);
        T Update(T entity);
        T Delete(int id);
    }
}
