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
            return _context.Produto.AsNoTracking().ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produto.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);

            if(produto == null)
            {
                return NotFound();
            }
            return produto;
        }
    }
}
