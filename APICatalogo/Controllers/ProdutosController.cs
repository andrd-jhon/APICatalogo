using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
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
        private readonly IProdutoRepository _produtoRepository;

        public ProdutosController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            return Ok(_produtoRepository.GetById(id));
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, _produtoRepository.CreateProduto(produto));
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            var deletado = _produtoRepository.UpdateProduto(produto);

            if (deletado)
                return Ok();

            return StatusCode(500);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            bool deletado = _produtoRepository.DeleteProduto(id);

            if (deletado)
                return Ok();

            return StatusCode(500);
        }
    }
}
