using Application.Commands;
using Application.Repositories.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Handlers;

public class ExcluirProdutoHandler : IRequestHandler<ExcluirProdutoCommand>
{
    private readonly IProdutoRepository _produtoRepository;

    public ExcluirProdutoHandler(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task Handle(ExcluirProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(request.Id);
        if (produto == null)
        {
            throw new ExcecaoNaoEncontrado("Produto não encontrado");
        }

        await _produtoRepository.ExcluirAsync(request.Id);
    }
}