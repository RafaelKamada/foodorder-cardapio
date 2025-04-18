using MediatR;
using Application.DTOs;

namespace Application.Queries;

public class ObterProdutosPorIdQuery : IRequest<List<ProdutoDto>>
{
    public List<int> Ids { get; set; }

    public ObterProdutosPorIdQuery(List<int> ids)
    {
        Ids = ids;
    }
}