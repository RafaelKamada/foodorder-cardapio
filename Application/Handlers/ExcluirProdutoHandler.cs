using Application.Commands;
using Application.Repositories.Interfaces;
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
        await _produtoRepository.ExcluirAsync(request.Id);
    }
}