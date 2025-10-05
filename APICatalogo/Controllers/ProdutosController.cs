using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("/primeiro")] // acessivel por meio do caminho ...produtos/primeiro
        //[HttpGet("teste")]
        //[HttpGet("/segundo")]
        //[HttpGet("{valor:alpha:length(5)}")]
        public ActionResult<Produto> GetPrimeiro(string valor)
        {
            var produtos = _context.Produtos.FirstOrDefault();

            throw new Exception("Exceção teste.");

            if (produtos is null)
                return NotFound();

            return produtos;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get2()
        {
            var produtos = await _context.Produtos.ToListAsync();

            if (produtos is null)
                return NotFound();

            return produtos;
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> Get([FromQuery] int id)
        {
            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound();

            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto == null)
                return BadRequest();

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
                return BadRequest();

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete (int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound("Produto não localizado.");

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
