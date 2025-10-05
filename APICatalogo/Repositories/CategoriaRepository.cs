using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        public readonly AppDbContext _dbContext;
        public CategoriaRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Categoria CreateCategoria(Categoria categoria)
        {
            if (categoria is null)
                throw new ArgumentNullException(nameof(categoria));

            _dbContext.Add(categoria);
            _dbContext.SaveChanges();

            return categoria;
        }

        public Categoria DeleteCategoria(int id)
        {
            var categoria = _dbContext.Categorias.Find(id);
            
            if (categoria is null)
                throw new NullReferenceException(nameof(categoria));

            _dbContext.Categorias.Remove(categoria);
            _dbContext.SaveChanges();

            return categoria;
        }

        public Categoria GetCategoria(int id)
        {
            var categoria = _dbContext.Categorias.FirstOrDefault(p => p.CategoriaId == id);

            if (categoria is null)
                throw new NullReferenceException();

            return categoria;
        }

        public IEnumerable<Categoria> GetCategorias()
        {
            return _dbContext.Categorias.ToList();
        }

        public Categoria UpdateCategoria(Categoria categoria)
        {
            if (categoria is null)
                throw new ArgumentNullException();

            _dbContext.Entry(categoria).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return categoria;
        }
    }
}
