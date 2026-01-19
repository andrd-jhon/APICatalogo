using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Interfaces;
using APICatalogo.Logging;
using APICatalogo.Models;
using APICatalogo.Pagination;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using X.PagedList;

namespace APICatalogo.Controllers
{
    //[EnableCors("OrigensComAcessoPermitido")]
    [Route("[controller]")]
    [ApiController]
    [EnableRateLimiting("fixedWindow")]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _unityOfWork;
        private readonly CustomerLogger _logger;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork unityOfWork, IMapper mapper)
        {
            _unityOfWork = unityOfWork;
            _mapper = mapper;
        }

        //[DisableRateLimiting]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            var categorias = await _unityOfWork.CategoriaRepository.GetAllAsync();

            if (categorias is null)
                return NotFound();

            var categoriasDTO = categorias.ToCategoriaDTOList();

            return Ok(categoriasDTO);
        }

        //[Authorize(Policy = "AdminOrOwner")]
        [DisableCors]
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _unityOfWork.CategoriaRepository.Get(p => p.CategoriaId == id);

            if (categoria is null)
                return StatusCode(404);

            var categoriaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaDTO);
        }

        [Authorize(Policy = "OwnerOnly")]
        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null)
            {
                _logger.LogWarning($"Dados inválidos.");
                return BadRequest();
            }

            var categoria = categoriaDTO.ToCategoria();

            var categoriaCriada = _unityOfWork.CategoriaRepository.Create(categoria);

            await _unityOfWork.CommitAsync();

            var retornoCategoriaDTO = categoriaCriada.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria", new { id = retornoCategoriaDTO.CategoriaId }, retornoCategoriaDTO);
        }

        [Authorize(Policy = "AdminOrOwner")]
        [HttpPut]
        public async Task<ActionResult<CategoriaDTO>> Put(CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null)
            {
                _logger.LogWarning($"Dados inválidos.");
                return BadRequest();
            }

            var categoria = categoriaDTO.ToCategoria();

            _unityOfWork.CategoriaRepository.Update(categoria);
            await _unityOfWork.CommitAsync();

            var categoriaAtualizadaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaAtualizadaDTO);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "OwnerOnly")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            if (id <= 0)
                return BadRequest();

            var categoria = _unityOfWork.CategoriaRepository.Delete(id);

            if (categoria is null)
            {
                _logger.LogWarning($"Dados inválidos.");
                return BadRequest();
            }

            await _unityOfWork.CommitAsync();

            var categoriaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaDTO);
        }

        [AllowAnonymous]
        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAsync([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = await _unityOfWork.CategoriaRepository.GetCategoriasAsync(categoriasParameters);

            return ObterCategorias(categorias);
        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(IPagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.Count,
                categorias.PageSize,
                categorias.PageCount,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriasDTO = categorias.ToCategoriaDTOList();

            return Ok(categoriasDTO);
        }

        [AllowAnonymous]
        [HttpGet("filter/name/pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltradas([FromQuery] CategoriasFiltroNome categoriasFiltro)
        {
            var categorias = await _unityOfWork.CategoriaRepository.GetCategoriasFiltroNomeAsync(categoriasFiltro);
            var teste = "commit teste";
            return ObterCategorias(categorias);
        }
    }
}
