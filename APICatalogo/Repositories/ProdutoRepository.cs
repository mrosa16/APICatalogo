using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

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


        public PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters)
        {
            var produtos = GetAll().OrderBy(p => p.ProdutoId).AsQueryable();
            var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParameters.PageNumber, produtosParameters.PageSize);
            return produtosOrdenados;
        }

        public PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroParams)
        {
            var produtos = GetAll().AsQueryable();
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

            var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, produtosFiltroParams.PageNumber,
                                                                                              produtosFiltroParams.PageSize);
            return produtosFiltrados;


        }

        public IEnumerable<Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(c => c.CategoriaId == id);
        }

        
    } 
}