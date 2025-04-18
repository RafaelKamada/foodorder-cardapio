using Application.Commands;
using Application.DTOs;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Domain.Enums;

namespace Api.Controllers
{
    [ApiController]
    [Route("Produtos")]
    public class ProdutosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProdutosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("ObterTodos")]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> ObterTodos()
        {
            var produtos = await _mediator.Send(new ObterTodosProdutosQuery());
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoDto>> ObterPorId(int id)
        {
            var produto = await _mediator.Send(new ObterProdutoQuery(id));
            if (produto == null)
            {
                return NotFound();
            }
            return Ok(produto);
        }


        [HttpGet("ObterPorIds")]
        public async Task<ActionResult<ProdutoDto>> ObterProdutosPorIds([FromBody] List<int> ids)
        {
            var produtos = await _mediator.Send(new ObterProdutosPorIdQuery(ids));
            if (produtos == null)
            {
                return NotFound();
            }
            return Ok(produtos);
        }


        [HttpGet("ObterCategorias")]
        public ActionResult<IEnumerable<string>> ObterCategorias()
        {
            return Ok(Enum.GetNames(typeof(CategoriaTipo)));
        }

        [HttpGet("ObterPorCategoria/{categoria}")]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> ObterPorCategoria(string categoria)
        {
            var produtos = await _mediator.Send(new ObterProdutoPorCategoriaQuery(categoria));
            if (produtos == null)
            {
                return NotFound();
            }
            return Ok(produtos);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDto>> Criar([FromBody] CriarProdutoCommand command)
        {
            var produto = await _mediator.Send(command);
            return CreatedAtAction(nameof(ObterPorId), new { id = produto.Id }, produto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProdutoDto>> Atualizar(int id, [FromBody] AtualizarProdutoCommand command)
        {
            if (id != command.IdSequencial)
            {
                return BadRequest();
            }

            var produto = await _mediator.Send(command);
            return Ok(produto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            await _mediator.Send(new ExcluirProdutoCommand { Id = id });
            return NoContent();
        }
    }
}
