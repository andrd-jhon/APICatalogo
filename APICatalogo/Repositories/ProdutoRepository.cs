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

        public async Task<IEnumerable<Produto>> GetProdutosByCategoriaId(int id)
        {
            var produtos = await GetAllAsync();

            var categoriasOrdenadas = produtos.Where(p => p.CategoriaId == id);

            return categoriasOrdenadas;
        }
        
        public Task<PagedList<Produto>> GetProdutos (ProdutosParameters produtosParameters)
        {
            var produtos = Query().OrderBy(p => p.ProdutoId).AsQueryable();

            var produtosOrdenados = PagedList<Produto>.ToPagedListAsync(produtos, produtosParameters.PageNumber, produtosParameters.PageSize);

            return produtosOrdenados;
        }

        public Task<PagedList<Produto>> GetProdutosFiltroPreco (ProdutosFIltroPreco produtosFiltroParameters)
        {
            var produtos = Query().AsQueryable();

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
            }

            var produtosFiltrados = PagedList<Produto>.ToPagedListAsync(produtos, produtosFiltroParameters.PageNumber, produtosFiltroParameters.PageSize);

            return produtosFiltrados;
        }
    }
}
