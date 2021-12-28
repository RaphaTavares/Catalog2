using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [Route("/api/[Controller]")]
    [ApiController]
    public class ProdutosController: ControllerBase
    {

        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext contexto)
        {
            _context = contexto;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            try
            {
                return await _context.Produto.AsNoTracking().ToListAsync();

            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter os produtos");
            }
        }

        [HttpGet("{id:int:min(1)}", Name  = "ObterProduto")]
        public async Task<ActionResult<Produto>> Get(int id)
        {
            try
            {
                var produto = await _context.Produto.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);

                if (produto == null)
                {
                    return NotFound($"Produto com id={id} não encontrado");
                }
                return produto;
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar obter o produto de id={id}");
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Produto produto)
        {
            try
            {
                await _context.Produto.AddAsync(produto);
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);

            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar alterar o produto de id={produto.ProdutoId}");
            }
            }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                {
                    return BadRequest("Id inexistente no banco");
                }

                //_context.Entry(produto).State = EntityState.Modified;
                _context.Produto.Update(produto);
                await _context.SaveChangesAsync();
                return Ok("Produto atualizado com sucesso");
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Não foi possível alterar o produto");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Produto>> Delete(int id)
        {
            try
            {
                var produto = await _context.Produto.FirstOrDefaultAsync(p => p.ProdutoId == id);
                // var produto = _context.Produto.Find(id);

                if (produto is null)
                {
                    return NotFound("Produto não existe");
                }

                _context.Produto.Remove(produto);
                _context.SaveChanges();
                return produto;
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Não foi possível deletar o produto");
            }
            
        }
    }
}
