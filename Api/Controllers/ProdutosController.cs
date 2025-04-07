using Application.Commands;
using Application.DTOs;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Domain.Enums;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProdutosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoDto>>> ObterTodos()
    {
        var produtos = await _mediator.Send(new ObterTodosProdutosQuery());
        return Ok(produtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProdutoDto>> ObterPorId(string id)
    {
        var produto = await _mediator.Send(new ObterProdutoQuery(id));
        if (produto == null)
        {
            return NotFound();
        }
        return Ok(produto);
    }

    [HttpGet("categorias")]
    public ActionResult<IEnumerable<string>> ObterCategorias()
    {
        return Ok(Enum.GetNames(typeof(CategoriaTipo)));
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoDto>> Criar([FromBody] CriarProdutoCommand command)
    {
        var produto = await _mediator.Send(command);
        return CreatedAtAction(nameof(ObterPorId), new { id = produto.Id }, produto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProdutoDto>> Atualizar(string id, [FromBody] AtualizarProdutoCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        var produto = await _mediator.Send(command);
        return Ok(produto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Excluir(string id)
    {
        await _mediator.Send(new ExcluirProdutoCommand { Id = id });
        return NoContent();
    }
}