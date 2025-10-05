using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
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
        private readonly ICategoriaRepository _categoriaRepository;
        

        public CategoriasController(IConfiguration configuration, ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return Ok(_categoriaRepository.GetCategorias());
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            return Ok(_categoriaRepository.GetCategoria(id));
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, _categoriaRepository.CreateCategoria(categoria));
        }

        [HttpPut]
        public ActionResult Put(Categoria categoria)
        {
            return Ok(_categoriaRepository.UpdateCategoria(categoria));
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            return Ok(_categoriaRepository.DeleteCategoria(id));
        }
    }
}
