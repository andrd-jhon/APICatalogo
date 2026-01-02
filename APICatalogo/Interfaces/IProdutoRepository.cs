using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutosByCategoriaId(int id);
        Task<IPagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters);
        Task<IPagedList<Produto>> GetProdutosFiltroPreco(ProdutosFIltroPreco produtosFiltroParameters);
    }
}
