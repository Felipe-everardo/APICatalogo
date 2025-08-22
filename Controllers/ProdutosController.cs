using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> Get2()
    {
        return await _context.Produtos.AsNoTracking().ToListAsync();
    }

    // produtos/id
    [HttpGet("{id}, Name = ObterProduto")]
    public async Task<ActionResult<Produto>> Get([FromQuery]int id)
    {
        
        var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);

        if (produto is null)
        {
            return NotFound("Produto não encontrado...");
        }
        return produto;
    }

    // produtos
    [HttpPost]
    public ActionResult Post([FromBody]Produto produto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Produtos.Add(produto);
        _context.SaveChanges();

        return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
    }

    // produtos/id
    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
        {
            return BadRequest();
        }

        _context.Entry(produto).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(produto);
    }

    
    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

        if (produto is null)
        {
            return NotFound("Produto não localizado...");
        }
        _context.Produtos.Remove(produto);
        _context.SaveChanges();

        return Ok(produto);
    }
}
