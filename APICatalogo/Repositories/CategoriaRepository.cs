using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext dbContext): base(dbContext)
        {
        }
    }
}
