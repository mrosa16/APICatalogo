using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnityOfWork _uof;
        private readonly ILogger _logger;

        public CategoriasController(IUnityOfWork uof, ILogger<CategoriasController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();
            return Ok(categorias);  
        }
        
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            //throw new Exception("Exceção ao retornar a categoria pelo ID");

            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);
            _logger.LogInformation($"=====Get api/categoria/id = {id} =");
            if (categoria is null)
            {
                _logger.LogInformation($"=====Get api/categoria/id = {id} === NOT FOUND");
                return NotFound("Categoria não encontrada");
            }

            return Ok(categoria);
        }

        [HttpPost]

        public ActionResult Post(Categoria categoria) 
        {
            if (categoria == null)
            {
                return BadRequest();
            }

            var categoriaCriado = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();
            

            return new CreatedAtRouteResult("ObterCategoria",
                   new { id = categoria.CategoriaId}, categoria
                    
                );
        }


        [HttpPut("{id:int}")]
       
        public ActionResult Put(int id, Categoria categoria)
        {
            if(id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _uof.CategoriaRepository.Update(categoria);

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound("Categoria não localizado");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

            return Ok(categoria);
        }
    }
}
