using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Interfaces;
using APICatalogo.Logging;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _unityOfWork;
        private readonly CustomerLogger _logger;

        public CategoriasController(IUnitOfWork unityOfWork)
        {
            _unityOfWork = unityOfWork;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categorias = _unityOfWork.CategoriaRepository.GetAll();

            if (categorias is null)
                return NotFound();

            var categoriasDTO = categorias.ToCategoriaDTOList();

            return Ok(categoriasDTO);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _unityOfWork.CategoriaRepository.Get(p => p.CategoriaId == id);

            if (categoria is null)
                return StatusCode(404);

            var categoriaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaDTO);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null)
            {
                _logger.LogWarning($"Dados inválidos.");
                return BadRequest();
            }

            var categoria = categoriaDTO.ToCategoria();

            var categoriaCriada = _unityOfWork.CategoriaRepository.Create(categoria);

            _unityOfWork.Commit();

            var retornoCategoriaDTO = categoriaCriada.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria", new { id = retornoCategoriaDTO.CategoriaId }, retornoCategoriaDTO);
        }

        [HttpPut]
        public ActionResult<CategoriaDTO> Put(CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null)
            {
                _logger.LogWarning($"Dados inválidos.");
                return BadRequest();
            }

            var categoria = categoriaDTO.ToCategoria();

            _unityOfWork.CategoriaRepository.Update(categoria);
            _unityOfWork.Commit();

            var categoriaAtualizadaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaAtualizadaDTO);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _unityOfWork.CategoriaRepository.Get(p => p.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Dados inválidos.");
                return BadRequest();
            }
            _unityOfWork.Commit();

            var categoriaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaDTO);
        }
    }
}
