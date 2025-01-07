using APICatalogo.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository
    {
        IQueryable<Produto> GetProdutos();

        Produto GetProduto(int id);
        Produto Create (Produto produto);

        bool Update(Produto produto);

        bool Delete(int id);
    }
}
