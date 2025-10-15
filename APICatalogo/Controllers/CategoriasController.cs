using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Filters;
using APICatalogo.Interfaces;
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

            var categoriasDTO = new List<CategoriaDTO>();

            foreach (var categoria in categorias)
            {
                var categoriaDTO = new CategoriaDTO()
                {
                    CategoriaId = categoria.CategoriaId,
                    Nome = categoria.Nome,
                    ImagemUrl = categoria.ImagemUrl
                };

                categoriasDTO.Add(categoriaDTO);
            }

            return Ok();
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _unityOfWork.CategoriaRepository.Get(p => p.CategoriaId == id);

            if (categoria is null)
                return StatusCode(404);

            //MAPEAMENTO MANUAL DE DTO
            var categoriaDTO = new CategoriaDTO()
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
            };

            return Ok(categoriaDTO);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDTO)
        {
            var categoria = new Categoria()
            {
                CategoriaId = categoriaDTO.CategoriaId,
                Nome = categoriaDTO.Nome,
                ImagemUrl = categoriaDTO.ImagemUrl
            };

            var categoriaCriada = _unityOfWork.CategoriaRepository.Create(categoria);
            _unityOfWork.Commit();

            var retornoCategoriaDTO = new CategoriaDTO()
            {
                CategoriaId = categoriaCriada.CategoriaId,
                Nome = categoriaCriada.Nome,
                ImagemUrl = categoriaCriada.ImagemUrl
            };

            return new CreatedAtRouteResult("ObterCategoria", new { id = retornoCategoriaDTO.CategoriaId }, retornoCategoriaDTO);
        }

        [HttpPut]
        public ActionResult<CategoriaDTO> Put(CategoriaDTO categoriaDTO)
        {
            var categoria = new Categoria()
            {
                CategoriaId = categoriaDTO.CategoriaId,
                Nome = categoriaDTO.Nome,
                ImagemUrl = categoriaDTO.ImagemUrl
            };

            _unityOfWork.CategoriaRepository.Update(categoria);
            _unityOfWork.Commit();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _unityOfWork.CategoriaRepository.Delete(id);
            _unityOfWork.Commit();

            var categoriaDTO = new CategoriaDTO()
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
            };

            return Ok(categoriaDTO);
        }

        [HttpGet("Teste")]
        public ActionResult TesteFront()
        {
            return Ok(new { mensagem = "teste" });
        }

        [HttpPost("Teste")]
        public ActionResult TesteFront(string nome)
        {
            return Ok(new { mensagem = "teste"});
        }

    }

}
