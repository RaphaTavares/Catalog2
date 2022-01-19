using APICatalogo.DTOs;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [Route("/api/[Controller]")]
    [ApiController]
    public class ProdutosController: ControllerBase
    {

        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uof = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPrecos()
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorPreco();

            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDto;
        }


        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            try
            {
                var produtos = await _uof.ProdutoRepository.GetProdutos(produtosParameters);

                var metadata = new
                {
                    produtos.TotalCount,
                    produtos.PageSize,
                    produtos.CurrentPage,
                    produtos.TotalPages,
                    produtos.HasNext,
                    produtos.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
                return produtosDto;

            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter os produtos");
            }
        }

        [HttpGet("{id:int:min(1)}", Name  = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            try
            {
                var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

                if (produto == null)
                {
                    return NotFound($"Produto com id={id} não encontrado");
                }

                var produtoDto = _mapper.Map<ProdutoDTO>(produto);
                return produtoDto;
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar obter o produto de id={id}");
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]ProdutoDTO produtoDto)
        {
            try
            {
                var produto = _mapper.Map<Produto>(produtoDto);

                 _uof.ProdutoRepository.Add(produto);
                 await _uof.Commit();

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);


                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produtoDTO);

            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar alterar o produto de id={produtoDto.ProdutoId}");
            }
            }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProdutoDTO produtoDto)
        {
            try
            {


                if (id != produtoDto.ProdutoId)
                {
                    return BadRequest("Id inexistente no banco");
                }

                var produto = _mapper.Map<Produto>(produtoDto);

                _uof.ProdutoRepository.Update(produto);
                await _uof.Commit();
                return Ok("Produto atualizado com sucesso");
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Não foi possível alterar o produto");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            try
            {
                var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                // var produto = _uof.Produto.Find(id);

                if (produto is null)
                {
                    return NotFound("Produto não existe");
                }
                
                _uof.ProdutoRepository.Delete(produto);
                await _uof.Commit();

                var produtoDto = _mapper.Map<ProdutoDTO>(produto);

                return produtoDto;
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Não foi possível deletar o produto");
            }
            
        }
    }
}
