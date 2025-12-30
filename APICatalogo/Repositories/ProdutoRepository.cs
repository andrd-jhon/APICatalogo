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

            var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParameters.PageNumber, produtosParameters.PageSize);

            return produtosOrdenados;
        }

        public PagedList<Produto> GetProdutosFiltroPreco (ProdutosFIltroPreco produtosFiltroParameters)
        {
            var produtos = Query().AsQueryable();

            var sql = produtos.ToQueryString();

            if (produtosFiltroParameters.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroParameters.PrecoCriterio))
            {
                if (produtosFiltroParameters.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco > produtosFiltroParameters.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroParameters.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco < produtosFiltroParameters.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroParameters.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco == produtosFiltroParameters.Preco.Value).OrderBy(p => p.Preco);
                }

                sql = produtos.ToQueryString();

            }

            var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, produtosFiltroParameters.PageNumber, produtosFiltroParameters.PageSize);

            return produtosFiltrados;
        }
    }
}
