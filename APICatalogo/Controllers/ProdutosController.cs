using APICatalogo.Context;
using APICatalogo.Interfaces;
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
        private readonly IRepository<Produto> _repository;
        private readonly IProdutoRepository _produtoRepository;

        public ProdutosController(IRepository<Produto> repository, IProdutoRepository produtoRepository)
        {
            _repository = repository;
            _produtoRepository = produtoRepository;
        }

        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosByCategoriaId(int id)
        {
            var produtos = _produtoRepository.GetProdutosByCategoriaId(id);

            if (produtos is null)
                return NotFound();

            return Ok(produtos);
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            return Ok(_repository.Get(p => p.ProdutoId == id));
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, _repository.Create(produto));
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            return Ok(_repository.Update(produto));
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {


            return Ok(_repository.Delete(id));
        }
    }
}
