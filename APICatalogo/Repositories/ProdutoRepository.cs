using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context): base(context)
        {
        }
        public IEnumerable<Produto> GetProdutosByCategoriaId(int id)
        {
            return GetAll().Where(p => p.CategoriaId == id);
        }
    }
}
