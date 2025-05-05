using MediatR;

namespace Application.Commands;

public class ExcluirProdutoCommand : IRequest
{
    public int Id { get; set; }
}