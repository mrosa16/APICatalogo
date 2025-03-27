using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {

        public ProdutoRepository(AppDbContext context) : base(context) { 
            
        }

        //public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters)
        //{
        //    int totalProdutos = _context.Produtos.Count();
        //    int totalPages = (int)Math.Ceiling((double)totalProdutos / produtosParameters.PageSize);

        //    if (produtosParameters.PageNumber > totalPages)
        //    {
        //        Console.WriteLine($"Página {produtosParameters.PageNumber} não existe. Total de páginas: {totalPages}");
        //        return new List<Produto>(); // Retorna uma lista vazia em vez de executar uma query inválida
        //    }

        //    int skip = (produtosParameters.PageNumber - 1) * produtosParameters.PageSize;

        //    Console.WriteLine($"Total de produtos: {totalProdutos}, Total de páginas: {totalPages}");
        //    Console.WriteLine($"PageNumber: {produtosParameters.PageNumber}, PageSize: {produtosParameters.PageSize}, Skip: {skip}");

        //    return _context.Produtos
        //        .OrderBy(p => p.Nome)
        //        .Skip(skip)
        //        .Take(produtosParameters.PageSize)
        //        .AsNoTracking()
        //        .ToList();
        //}


        public async Task< IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters)
        {
            var produtos = await GetAllAsync();
            var produtosOrdenados = produtos.OrderBy(p => p.ProdutoId).AsQueryable();
            var resultado = await produtosOrdenados.ToPagedListAsync(produtosParameters.PageNumber,
                                                           produtosParameters.PageSize);

            return resultado;
        }

        public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams)
        {
            var produtos = await GetAllAsync();
            if(produtosFiltroParams.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroParams.PrecoCriterio))
            {
                if(produtosFiltroParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase)) 
                {
                    produtos = produtos.Where(p => p.Preco > produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
                }
                else if(produtosFiltroParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {

                    produtos = produtos.Where(p => p.Preco < produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
                }
                else if(produtosFiltroParams.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco == produtosFiltroParams.Preco.Value).OrderBy(p =>p.Preco);
                }
            }

            var produtosFiltrados = await produtos.ToPagedListAsync(produtosFiltroParams.PageNumber,
                                                           produtosFiltroParams.PageSize);
            return produtosFiltrados;


        }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)

        {
            var produtos = await GetAllAsync();
            var produtosCategoria = produtos.Where(c => c.CategoriaId == id);
            return produtosCategoria;
        }

        
    } 
}