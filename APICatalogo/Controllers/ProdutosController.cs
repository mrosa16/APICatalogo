using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : Controller
    {
        private readonly IUnityOfWork _uof;

        public ProdutosController(IUnityOfWork uof)
        {
            _uof = uof;
        }

   


        [HttpGet("Produtos/{id}")]
        public ActionResult <IEnumerable<Produto>> GetProdutoCategoria(int id)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);
            
            if(produtos is null)
                    return NotFound();
            return Ok(produtos);

        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get2()
        {
            var produtos = _uof.ProdutoRepository.GetAll().ToList();

            if(produtos is null)
            {
                return NotFound();
            }
            return Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name="ObterProduto")]

        public ActionResult<Produto> Get(int id)
        {


            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);
            if (produto is null) 
            {
                return NotFound("Produto não encontrado");
            }
            return Ok(produto);
        }
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
            {
                return BadRequest();
            }
           
          var novoProduto =   _uof.ProdutoRepository.Create(produto);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterProduto", new {id = novoProduto.ProdutoId}, novoProduto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        { 
            if(id != produto.ProdutoId)
            {
                return BadRequest();
            }

           var produtoAtualizado = _uof.ProdutoRepository.Update(produto);


            return Ok(produtoAtualizado);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
           var deletado = _uof.ProdutoRepository.Get(p=> p.ProdutoId ==id);
            if(deletado is null) 
            {
                return NotFound("Produto não encontrado");
            }
         
                var produtoDeletado = _uof.ProdutoRepository.Delete(deletado);
            _uof.Commit();
                return Ok(produtoDeletado);
            

     

           
        }
    }

   

   
}
