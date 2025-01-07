using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Categoria> Categorias => _context.Categorias;
        public IEnumerable<Categoria> GetCategorias()
        {
            return _context.Categorias.ToList();
        }
        public Categoria GetCategoria(int id)
        {
            return _context.Categorias.FirstOrDefault(x => x.CategoriaId == id);
        }
        public Categoria Create(Categoria categoria)
        {
            if (categoria is null) 
                throw new ArgumentNullException(nameof(categoria));
            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return categoria;
        }
        public Categoria Update(Categoria categoria)
        {
            if (categoria is null)
                throw new ArgumentNullException(nameof(categoria));
            _context.Categorias.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return categoria;
        }
        public Categoria Delete(int id)
        {
            var categoria = _context.Categorias.Find(id);

            if (categoria is null)
                throw new ArgumentNullException(nameof(categoria));

            _context.Categorias.Remove(categoria);

            _context.SaveChanges();
            return categoria;
        }

    }
}
