using Application.Queries;
using Application.Repositories.Interfaces;
using Application.DTOs;
using MediatR;

namespace Application.Handlers;

public class ObterProdutosPorIdHandler : IRequestHandler<ObterProdutosPorIdQuery, List<ProdutoDto>>
{
    private readonly IProdutoRepository _produtoRepository;

    public ObterProdutosPorIdHandler(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<List<ProdutoDto>> Handle(ObterProdutosPorIdQuery request, CancellationToken cancellationToken)
    {
        var produtos = await _produtoRepository.ObterPorIdsAsync(request.Ids);
        if (produtos == null || produtos.Count == 0)
            return null;

        var produtosDto = produtos.Select(produto => new ProdutoDto
        {
            Id = produto.IdSequencial,
            Nome = produto.Nome,
            Tipo = produto.Tipo,
            Preco = produto.Preco,
            Descricao = produto.Descricao,
            TempoPreparo = produto.TempoPreparo,
            Imagens = produto.Imagens.Select(i => new ImagemDto
            {
                Id = i.Id,
                Data = i.Data,
                ProdutoId = i.ProdutoId
            }).ToList()
        }).ToList();

        return produtosDto;
    }
}