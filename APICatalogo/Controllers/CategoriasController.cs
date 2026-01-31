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
    [EnableCors("OrigensComAcessoPermitido")]
    [Route("[controller]")]
    [ApiController]
    [EnableRateLimiting("fixedWindow")]
    [Produces("application/json")]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _unityOfWork;
        private readonly CustomerLogger? _logger;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork unityOfWork, IMapper mapper)
        {
            _unityOfWork = unityOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtem uma lista de categorias
        /// </summary>
        /// <returns>Umas lista de objetos de Categoria</returns>

        //[DisableRateLimiting]
        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            var categorias = await _unityOfWork.CategoriaRepository.GetAllAsync();

            if (categorias is null)
                return NotFound();

            var categoriasDTO = categorias.ToCategoriaDTOList();

            return Ok(categoriasDTO);
        }

        /// <summary>
        /// Obtem uma categoria pelo seu Id
        /// </summary>
        /// <param name="id">
        /// <returns>Um objeto Categoria</returns>

        //[Authorize(Policy = "AdminOrOwner")]
        [DisableCors]
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _unityOfWork.CategoriaRepository.Get(p => p.CategoriaId == id);

            if (categoria is null)
                return StatusCode(404);

            var categoriaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaDTO);
        }

        /// <summary>
        /// Inclui uma nova categoria
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     POST api/categorias
        ///     {
        ///        "categoriaId": 1,
        ///        "nome": "categoria1",
        ///        "imagemUrl": "http://teste.net/1.jpg"
        ///     }
        /// </remarks>
        /// <param name="categoriaDto">objeto Categoria</param>
        /// <returns>O objeto Categoria incluida</returns>
        /// <remarks>Retorna um objeto Categoria incluído</remarks>

        [Authorize(Policy = "OwnerOnly")]
        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null)
            {
                _logger!.LogWarning($"Dados inválidos.");
                return BadRequest();
            }

            var categoria = categoriaDTO.ToCategoria();

            var categoriaCriada = _unityOfWork.CategoriaRepository.Create(categoria!);

            await _unityOfWork.CommitAsync();

            var retornoCategoriaDTO = categoriaCriada.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria", new { id = retornoCategoriaDTO!.CategoriaId }, retornoCategoriaDTO);
        }

        #pragma warning disable CS1591
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

            return ObterCategorias(categorias);
        }
    }
}
