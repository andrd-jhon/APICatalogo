using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        public readonly AppDbContext _context;
        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }
        public Produto CreateProduto(Produto produto)
        {
            if (produto is null)
                throw new NullReferenceException();

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return produto;
        }

        public bool DeleteProduto(int id)
        {
            var produto = _context.Produtos.Find(id);

            if (produto is not null)
            {
                _context.Produtos.Remove(produto);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public Produto GetById(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if (produto is null) 
                throw new NullReferenceException();

            return produto;
        }

        public IQueryable<Produto> GetProdutos()
        {
            return _context.Produtos;
        }

        public bool UpdateProduto(Produto produto)
        {
            if (produto is null)
                throw new NullReferenceException();

            if (_context.Produtos.Any(p => p.ProdutoId == produto.ProdutoId))
            {
                _context.Produtos.Update(produto);
                _context.SaveChanges(true);
                return true;
            }

            return false;
        }
    }
}
