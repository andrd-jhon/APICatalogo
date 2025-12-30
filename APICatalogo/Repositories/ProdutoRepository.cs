using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;

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
        
        public IEnumerable<Produto> GetProdutos (ProdutosParameters produtosParameters)
        {
            return GetAll()
                .OrderBy(p => p.Nome)
                .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize) 
                .Take(produtosParameters.PageSize)
                .ToList();
        }
    }
}
