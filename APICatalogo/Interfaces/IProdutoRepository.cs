using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutosByCategoriaId(int id);
        Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters);
        Task<PagedList<Produto>> GetProdutosFiltroPreco(ProdutosFIltroPreco produtosFiltroParameters);
    }
}
