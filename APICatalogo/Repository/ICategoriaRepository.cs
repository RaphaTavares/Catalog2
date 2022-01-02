using APICatalogo.Models;
using APICatalogo.Pagination;
using System.Collections.Generic;

namespace APICatalogo.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria> 
    {
        IEnumerable<Categoria> GetCategoriasProdutos();
        PagedList<Categoria> GetCategorias(CategoriasParameters categoriaParameters);
    }
}
