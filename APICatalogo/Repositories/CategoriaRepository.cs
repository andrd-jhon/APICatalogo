using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using X.PagedList;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext dbContext): base(dbContext)
        {
        }

        public Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters)
        {
            var categorias = Query().OrderBy(p => p.CategoriaId).AsQueryable();

            //var produtosOrdenados = PagedList<Categoria>.ToPagedListAsync(categorias, categoriasParameters.PageNumber, categoriasParameters.PageSize);
            var produtosOrdenados = categorias.ToPagedListAsync(categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return produtosOrdenados;
        }

        public Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasParameters)
        {
            var categorias = Query().AsQueryable();

            if (!string.IsNullOrEmpty(categoriasParameters.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriasParameters.Nome));
            }

            //var categoriasFiltradas = PagedList<Categoria>.ToPagedListAsync(categorias, categoriasParameters.PageNumber, categoriasParameters.PageSize);
            var categoriasFiltradas = categorias.ToPagedListAsync(categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return categoriasFiltradas;
        }
    }
}
