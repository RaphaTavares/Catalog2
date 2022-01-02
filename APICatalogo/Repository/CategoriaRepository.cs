﻿using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace APICatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {

        public CategoriaRepository(AppDbContext contexto): base(contexto) { }

        public PagedList<Categoria> GetCategorias(CategoriasParameters categoriaParameters)
        {
            return PagedList<Categoria>.ToPagedList(Get().OrderBy(on => on.Nome), categoriaParameters.PageNumber, categoriaParameters.PageSize);
        }

        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            return Get().Include(categoria => categoria.Produtos);
        }
    }
}
