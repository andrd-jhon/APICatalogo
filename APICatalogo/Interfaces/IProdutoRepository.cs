using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosByCategoriaId(int id);
        //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters);
        PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters);
        PagedList<Produto> GetProdutosFiltroPreco(ProdutosFIltroPreco produtosFiltroParameters);
    }
}
