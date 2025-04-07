using Application.Queries;
using Application.Repositories.Interfaces;
using Application.DTOs;
using MediatR;

namespace Application.Handlers;

public class ObterTodosProdutosHandler : IRequestHandler<ObterTodosProdutosQuery, IEnumerable<ProdutoDto>>
{
    private readonly IProdutoRepository _produtoRepository;

    public ObterTodosProdutosHandler(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<IEnumerable<ProdutoDto>> Handle(ObterTodosProdutosQuery request, CancellationToken cancellationToken)
    {
        var produtos = await _produtoRepository.ObterTodosAsync();
        return produtos.Select(p => new ProdutoDto
        {
            Id = p.Id,
            Nome = p.Nome,
            Tipo = p.Tipo,
            Preco = p.Preco,
            Descricao = p.Descricao,
            TempoPreparo = p.TempoPreparo,
            Imagens = p.Imagens.Select(i => new ImagemDto
            {
                Id = i.Id,
                Data = i.Data,
                ProdutoId = i.ProdutoId
            }).ToList()
        });
    }
}