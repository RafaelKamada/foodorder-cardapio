using Application.Queries;
using Application.Repositories.Interfaces;
using Application.DTOs;
using MediatR;

namespace Application.Handlers;

public class ObterProdutoHandler : IRequestHandler<ObterProdutoQuery, ProdutoDto>
{
    private readonly IProdutoRepository _produtoRepository;

    public ObterProdutoHandler(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<ProdutoDto> Handle(ObterProdutoQuery request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(request.Id);
        if (produto == null)
            return null;

        return new ProdutoDto
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
        };
    }
}