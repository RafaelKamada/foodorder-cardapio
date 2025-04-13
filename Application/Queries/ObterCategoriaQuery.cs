using MediatR;
using Application.DTOs;

namespace Application.Queries;

public class ObterCategoriaQuery : IRequest<ProdutoDto>
{
    public string Categoria { get; set; }

    public ObterCategoriaQuery(string categoria)
    {
        Categoria = categoria;
    }
}