using APICatalogo.Context;
using APICatalogo.Models;
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
            _context = contexto
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _context.Categoria.AsNoTracking().ToList();
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _context.Categoria.AsNoTracking().FirstOrDefault(categoria => categoria.CategoriaId == id);

            if(categoria == null)
            {
                return NotFound();
            }
            return categoria;
        }

        [HttpPost]
        public ActionResult Post([FromBody]Categoria categoria)
        {
            _context.Categoria.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            if(id!= categoria.CategoriaId)
            {
                return BadRequest();
            }
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var categoria = _context.Categoria.FirstOrDefault(categoria => categoria.CategoriaId==id);

            if(categoria is null)
            {
                return NotFound();
            }
            _context.Categoria.Remove(categoria);
            _context.SaveChanges();
            return categoria;
        }
    }
}
