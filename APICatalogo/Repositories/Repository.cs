using APICatalogo.Context;
using APICatalogo.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APICatalogo.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }
        public IQueryable<T> Query()
        {
            return _context.Set<T>().AsNoTracking();
        }
        public async Task<T?> Get(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
        }
        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);
            //_context.SaveChanges();

            return entity;
        }
        public async Task<T> Update(T entity)
        {
            _context.Set<T>().Update(entity);

            await _context.SaveChangesAsync();

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
