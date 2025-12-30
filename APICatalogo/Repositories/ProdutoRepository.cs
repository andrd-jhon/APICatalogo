using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

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
        
        public PagedList<Produto> GetProdutos (ProdutosParameters produtosParameters)
        {
            var produtos = Query().OrderBy(p => p.ProdutoId).AsQueryable();

            var sql = produtos.ToQueryString();

            var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParameters.PageNumber, produtosParameters.PageSize);

            return produtosOrdenados;
        }
    }
}
