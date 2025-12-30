using APICatalogo.Context;
using APICatalogo.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APICatalogo.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
        }
        public IReadOnlyList<T> GetAll()
        {
            return [.. _context.Set<T>().AsNoTracking()];
        }
        public IQueryable<T> Query()
        {
            return _context.Set<T>().AsNoTracking();
        }
        public T? Get(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().AsNoTracking().FirstOrDefault(predicate);
        }
        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);
            //_context.SaveChanges();

            return entity;
        }
        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            //_context.Entry(entity).State = EntityState.Modified;
            //_context.SaveChanges();

            return entity;
        }
        public T Delete(int id)
        {
            //HARD DELETE
            var entity = _context.Find<T>(id);

            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                //_context.SaveChanges();
            }

            return entity;
        }
    }
}
