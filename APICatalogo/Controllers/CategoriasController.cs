using APICatalogo.Context;
using APICatalogo.DTO;
using APICatalogo.DTO.Mappings;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;

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
        [Authorize]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            var categorias = await _uof.CategoriaRepository.GetAllAsync();
            if(categorias is null)
                return NotFound();

            var categoriasDto = categorias.ToCategoriaDTOList();
            return Ok(categoriasDto);  
        }
        
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            //throw new Exception("Exceção ao retornar a categoria pelo ID");

            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);
            _logger.LogInformation($"=====Get api/categoria/id = {id} =");
            if (categoria is null)
            {
                _logger.LogInformation($"=====Get api/categoria/id = {id} === NOT FOUND");
                return NotFound("Categoria não encontrada");
            }

            var categoriaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaDTO);
        }

        [HttpPost]

        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDto) 
        {
            if (categoriaDto == null)
            {
                return BadRequest();
            }

            var categoria = categoriaDto.ToCategoria();

            var categoriaCriado = _uof.CategoriaRepository.Create(categoria);
           await _uof.CommitAssync();

            var novacategoriaDTO = categoriaCriado.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria",
                   new { id = novacategoriaDTO.CategoriaId}, novacategoriaDTO

                );
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id={id} não encontrada...");
                return NotFound($"Categoria com id={id} não encontrada...");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.CommitAssync();

            var categoriaExcluidaDto = categoriaExcluida.ToCategoriaDTO();

            return Ok(categoriaExcluidaDto);
            }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            var categoria = categoriaDto.ToCategoria();

            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
           await _uof.CommitAssync();

            var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDTO();

            return Ok(categoriaAtualizadaDto);
        }
        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasAsync(categoriasParameters);
            return ObterCategoria(categorias);

        }

        [HttpGet("filter/nome/pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriaFiltradas([FromQuery] CategoriasFiltroNome categoriasFiltro)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasFiltroNomeAsync(categoriasFiltro);
            return ObterCategoria(categorias);
        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategoria(IPagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.Count,
                categorias.PageSize,
                categorias.PageCount,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage,
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriaDTO = categorias.ToCategoriaDTOList();

            return Ok(categoriaDTO);
        }
    }
}
