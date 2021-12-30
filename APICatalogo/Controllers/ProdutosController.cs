using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace APICatalogo.Controllers
{
    [Route("/api/[Controller]")]
    [ApiController]
    public class ProdutosController: ControllerBase
    {

        private readonly IUnitOfWork _uof;
        public ProdutosController(IUnitOfWork unitOfWork)
        {
            _uof = unitOfWork;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPrecos()
        {
            return _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
        }


        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                return _uof.ProdutoRepository.Get().ToList();

            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter os produtos");
            }
        }

        [HttpGet("{id:int:min(1)}", Name  = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            try
            {
                var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

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
                 _uof.ProdutoRepository.Add(produto);
                _uof.Commit();

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

                //_uof.Entry(produto).State = EntityState.Modified;
                _uof.ProdutoRepository.Update(produto);
                 _uof.Commit();
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
                var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                // var produto = _uof.Produto.Find(id);

                if (produto is null)
                {
                    return NotFound("Produto não existe");
                }

                _uof.ProdutoRepository.Delete(produto);
                _uof.Commit();
                return produto;
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Não foi possível deletar o produto");
            }
            
        }
    }
}
