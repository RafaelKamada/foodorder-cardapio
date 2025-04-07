using MediatR;
using Application.DTOs;

namespace Application.Queries;

public class ObterProdutoQuery : IRequest<ProdutoDto>
{
    public string Id { get; set; }

    public ObterProdutoQuery(string id)
    {
        Id = id;
    }
}