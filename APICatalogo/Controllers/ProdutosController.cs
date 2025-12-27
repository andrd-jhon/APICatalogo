using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly IMapper _mapper;

        public ProdutosController(/*IRepository<Produto> repository,*/ IProdutoRepository produtoRepository, IUnitOfWork uow, IMapper mapper)
        {
            //_repository = repository;
            _produtoRepository = produtoRepository;
            _uow = uow;
            _mapper = mapper;
        }

        [HttpPatch("{id}/UpdatePartial")]
        public ActionResult<ProdutoDTOUpdateResponse> Patch (int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
        {
            if (patchProdutoDTO is null || id <= 0)
                return BadRequest();

            var produto = _uow.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound();

            var produtoDTOUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

            patchProdutoDTO.ApplyTo(produtoDTOUpdateRequest, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(produtoDTOUpdateRequest))
                return BadRequest(ModelState);

            _mapper.Map(produtoDTOUpdateRequest, produto);
            _uow.ProdutoRepository.Update(produto);
            _uow.Commit();

            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
        }

        [HttpGet("Categoria/{id}")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosByCategoriaId(int id)
        {
            var produtos = _produtoRepository.GetProdutosByCategoriaId(id);

            if (produtos is null)
                return NotFound();

            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            var produtos = _uow.ProdutoRepository.GetAll();

            if (produtos is null)
                return NotFound();

            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = _produtoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDTO);
        }

        [HttpPost]
        public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDTO)
        {
            if (produtoDTO is null)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDTO);

            produto = _uow.ProdutoRepository.Create(produto);

            _uow.Commit();

            produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto", new { id = produtoDTO.ProdutoId }, produtoDTO);
        }

        //[HttpPut("{id:int}")]
        //public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDTO)
        //{
        //    if (id != produtoDTO.ProdutoId)
        //        return BadRequest();

        //    var produto = _mapper.Map<Produto>(produtoDTO);

        //    var novoProduto = _produtoRepository.Update(produto);

        //    _uow.Commit();

        //    produtoDTO = _mapper.Map<ProdutoDTO>(novoProduto);

        //    return Ok(produtoDTO);
        //}

        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.ProdutoId)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDTO);

            produto = _produtoRepository.Update(produto);

            _uow.Commit();

            produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDTO);
        }


        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            if (id >= 0)
                return BadRequest("Código de produto inválido!");

            var produto = _produtoRepository.Delete(id);

            if (produto is null)
                return NotFound("Produto não encontrado!");

            _uow.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDTO);
        }
    }
}
