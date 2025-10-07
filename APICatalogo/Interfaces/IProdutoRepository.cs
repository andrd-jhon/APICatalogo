using APICatalogo.Models;

namespace APICatalogo.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosByCategoriaId(int id);
    }
}
