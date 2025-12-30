using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext dbContext): base(dbContext)
        {
        }

        public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters)
        {
            var categorias = Query().OrderBy(p => p.CategoriaId).AsQueryable();

            var produtosOrdenados = PagedList<Categoria>.ToPagedList(categorias, categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return produtosOrdenados;
        }

        public PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasParameters)
        {
            var categorias = Query().AsQueryable();

            if (!string.IsNullOrEmpty(categoriasParameters.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriasParameters.Nome));
            }

            var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return categoriasFiltradas;
        }
    }
}
