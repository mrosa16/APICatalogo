using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
    
      public CategoriaRepository(AppDbContext context) : base(context)
        {
        }
        public PagedList<Categoria> GetCategorias(CategoriasParameters categoriaParams)
        {
            var categorias = GetAll().OrderBy(p => p.CategoriaId).AsQueryable();
            var categoriasOrdenados = PagedList<Categoria>.ToPagedList(categorias,

                categoriaParams.PageNumber, categoriaParams.PageSize);
            return categoriasOrdenados;
        }

        public PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categeriaParams)
        {

            var categorias = GetAll().AsQueryable();
            if (!string.IsNullOrEmpty(categeriaParams.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categeriaParams.Nome));
            }

            var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, categeriaParams.PageNumber,
                categeriaParams.PageSize);

            return categoriasFiltradas;
        }
    }
}
