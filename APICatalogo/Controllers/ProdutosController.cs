using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        //private readonly IRepository<Produto> _repository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IUnitOfWork _uow;

        public ProdutosController(/*IRepository<Produto> repository,*/ IProdutoRepository produtoRepository, IUnitOfWork uow)
        {
            //_repository = repository;
            _produtoRepository = produtoRepository;
            _uow = uow;
        }

        [HttpGet("Categoria/{id}")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosByCategoriaId(int id)
        {
            var produtos = _produtoRepository.GetProdutosByCategoriaId(id);

            if (produtos is null)
                return NotFound();

            return Ok(produtos);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            var produtos = _uow.ProdutoRepository.GetAll();

            if (produtos is null)
                return NotFound();

            return Ok(produtos);
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            return Ok(_produtoRepository.Get(p => p.ProdutoId == id));
        }

        [HttpPost]
        public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDTO)
        {
            if (produtoDTO is null)
                return BadRequest();

            var produto = produtoDTO.ToProduto();

            var novoProduto = _uow.ProdutoRepository.Create(produto);
            _uow.Commit();

            return new CreatedAtRouteResult("ObterProduto", new { id = produtoDTO.ProdutoId }, _produtoRepository.Create(produto));
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDTO)
        {
            if (produtoDTO is null)
                return BadRequest();

            var produto = produtoDTO.ToProduto();
            var newProduto = _produtoRepository.Update(produto);
            _uow.Commit();

            return Ok(newProduto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produtoDTO = _produtoRepository.Delete(id).ToProdutoDTO();
            _uow.Commit();

            return Ok(produtoDTO);
        }
    }
}
