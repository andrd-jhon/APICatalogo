using APICatalogo.Context;
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
        private readonly IRepository<Categoria> _repository;
        
        public CategoriasController(IRepository<Categoria> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            return Ok(_repository.Get(p => p.CategoriaId == id));
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, _repository.Create(categoria));
        }

        [HttpPut]
        public ActionResult Put(Categoria categoria)
        {
            return Ok(_repository.Update(categoria));
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            return Ok(_repository.Delete(id));
        }
    }
}
