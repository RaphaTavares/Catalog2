using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace APICatalogo.Controllers
{
    [Route("/api/[Controller]")]
    [ApiController]
    public class CategoriasController: ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext contexto)
        {
            _context = contexto;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            try
            {
                return _context.Categoria.Include(x => x.Produtos).ToList();

            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter as categorias com produtos");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                return _context.Categoria.AsNoTracking().ToList();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar obter as categorias do banco de dados");
            }

        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _context.Categoria.AsNoTracking().FirstOrDefault(categoria => categoria.CategoriaId == id);

                if (categoria == null)
                {
                    return NotFound($"A categoria com id={id} não foi encontrada");
                }
                return categoria;
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter as categorias do banco de dados");
            }
            
        }

        [HttpPost]
        public ActionResult Post([FromBody]Categoria categoria)
        {
            try
            {
                _context.Categoria.Add(categoria);
                _context.SaveChanges();

                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
            }
            catch (System.Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar uma nova categoria");
            }
            }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {

            try
            {
                if (id != categoria.CategoriaId)
                {
                    return BadRequest($"não foi possível atualizar a categoria com id={id}");
                }
                _context.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok($"A categoria com id={id} foi atualizada com sucesso");
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar categoria");
            }
            
        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            try
            {
                var categoria = _context.Categoria.FirstOrDefault(categoria => categoria.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound();
                }
                _context.Categoria.Remove(categoria);
                _context.SaveChanges();
                return categoria;
            }
            catch (System.Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar categoria");
            }
            
        }
    }
}
