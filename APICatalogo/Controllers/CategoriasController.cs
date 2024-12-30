using APICatalogo.Context;
using APICatalogo.Models;
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
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public CategoriasController(AppDbContext context, ILogger<CategoriasController> logger)
        {
            _context = context;
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
            return _context.Categorias.Include(p => p.Produtos).Where(c => c.CategoriaId <= 5).ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            _logger.LogInformation("=====Get api/categorias ==");
            return _context.Categorias.AsNoTracking().ToList();
        }
        
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            //throw new Exception("Exceção ao retornar a categoria pelo ID");

            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
            _logger.LogInformation($"=====Get api/categoria/id = {id} =");
            if (categoria == null)
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

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

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

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound("Categoria não localizado");
            }

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
    }
}
