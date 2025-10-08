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
        private readonly IUnitOfWork _unityOfWork;
        
        public CategoriasController(IUnitOfWork unityOfWork)
        {
            _unityOfWork = unityOfWork;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return Ok(_unityOfWork.CategoriaRepository.GetAll());
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            return Ok(_unityOfWork.CategoriaRepository.Get(p => p.CategoriaId == id));
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            _unityOfWork.CategoriaRepository.Create(categoria);
            _unityOfWork.Commit();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut]
        public ActionResult Put(Categoria categoria)
        {
            _unityOfWork.CategoriaRepository.Update(categoria);
            _unityOfWork.Commit();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _unityOfWork.CategoriaRepository.Delete(id);
            _unityOfWork.Commit();

            return Ok(categoria);
        }
    }
}
