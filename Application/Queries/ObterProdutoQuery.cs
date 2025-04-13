using MediatR;
using Application.DTOs;

namespace Application.Queries;

public class ObterProdutoQuery : IRequest<ProdutoDto>
{
    public int Id { get; set; }

    public ObterProdutoQuery(int id)
    {
        Id = id;
    }
}