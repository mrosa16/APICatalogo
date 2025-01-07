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
        private readonly ICategoriaRepository _repository;
        private readonly ILogger _logger;

        public CategoriasController(ICategoriaRepository repository, ILogger<CategoriasController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("UsandoFromServices/{nome}")]
        public ActionResult<string> GetSaudacaoFromServices([FromServices] ImeuServico meuServico,
                                                             string nome)
        {
         return meuServico.Saudacao(nome);
        }
        [HttpGet("semUsarFromService/{nome}")]
        public ActionResult<string> GetSaudacaoSemUsarFromServices(ImeuServico meuServico,
                                                            string nome)
        {
            return meuServico.Saudacao(nome);
        }

        [HttpGet("produtos")]

        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()

        {
            _logger.LogInformation("=====Get api/categorias/produtos ==");
            var categorias = _repository.Categorias
        .Include(p => p.Produtos)
        .Where(c => c.CategoriaId <= 5)
        .ToList();

            return Ok(categorias); ;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _repository.GetCategorias();
            return Ok(categorias);  
        }
        
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            //throw new Exception("Exceção ao retornar a categoria pelo ID");

            var categoria = _repository.GetCategoria(id);
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

            var categoriaCriado = _repository.Create(categoria);

            

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

            _repository.Update(categoria);

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _repository.GetCategoria(id);

            if (categoria is null)
            {
                return NotFound("Categoria não localizado");
            }

            var categoriaExcluida = _repository.Delete(id);

            return Ok(categoria);
        }
    }
}
