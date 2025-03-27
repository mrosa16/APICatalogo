using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using X.PagedList;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters);
        Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters);
        Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams);
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);

    }
}
