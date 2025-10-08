using APICatalogo.Context;
using APICatalogo.Interfaces;

namespace APICatalogo.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProdutoRepository? _produtoRepository;
        private ICategoriaRepository? _categoriaRepository;

        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            {
                return _produtoRepository ??= new ProdutoRepository(_context);
            }
        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _categoriaRepository ??= new CategoriaRepository(_context);
                
                //if (_categoriaRepository == null)
                //{
                //    _categoriaRepository = new CategoriaRepository(_context);
                //}

                //return _categoriaRepository;
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
