using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace APICatalogo.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {
        }

        public DbSet <Categoria> categorias { get; set; }
        public DbSet <Produto> produtos { get; set; }
    }
}
