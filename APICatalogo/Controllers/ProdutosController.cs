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
    public class ProdutosController: ControllerBase
    {

        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext contexto)
        {
            _context = contexto;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                return _context.Produto.AsNoTracking().ToList();

            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter os produtos");
            }
        }

        [HttpGet("{id}", Name  = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            try
            {
                var produto = _context.Produto.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);

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
        public ActionResult Post([FromBody]Produto produto)
        {
            try
            {
                _context.Produto.Add(produto);
                _context.SaveChanges();

                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);

            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar alterar o produto de id={produto.ProdutoId}");
            }
            }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                {
                    return BadRequest("Id inexistente no banco");
                }

                _context.Entry(produto).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok("Produto atualizado com sucesso");
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Não foi possível alterar o produto");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            try
            {
                var produto = _context.Produto.FirstOrDefault(p => p.ProdutoId == id);
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
