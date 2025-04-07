using MediatR;

namespace Application.Commands;

public class ExcluirProdutoCommand : IRequest
{
    public string Id { get; set; }
}