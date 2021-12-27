using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutos()
        {
            try
            {
                return await _context.Categoria.Include(x => x.Produtos).ToListAsync();

            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter as categorias com produtos");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> Get()
        {
            try
            {
                return await _context.Categoria.AsNoTracking().ToListAsync();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar obter as categorias do banco de dados");
            }

        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            try
            {
                var categoria = await _context.Categoria.AsNoTracking().FirstOrDefaultAsync(categoria => categoria.CategoriaId == id);

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
        public async Task<ActionResult> Post([FromBody]Categoria categoria)
        {
            try
            {
                await _context.Categoria.AddAsync(categoria);
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
            }
            catch (System.Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar uma nova categoria");
            }
            }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Categoria categoria)
        {

            try
            {
                if (id != categoria.CategoriaId)
                {
                    return BadRequest($"não foi possível atualizar a categoria com id={id}");
                }
                _context.Entry(categoria).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok($"A categoria com id={id} foi atualizada com sucesso");
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar categoria");
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Categoria>> Delete(int id)
        {
            try
            {
                var categoria = await _context.Categoria.FirstOrDefaultAsync(categoria => categoria.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound();
                }
                _context.Categoria.Remove(categoria);
                await _context.SaveChangesAsync();
                return categoria;
            }
            catch (System.Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar categoria");
            }
            
        }
    }
}
