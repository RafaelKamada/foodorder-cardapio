using MediatR;
using Application.DTOs;

namespace Application.Queries;

public class ObterProdutosPorCategoriaQuery : IRequest<List<ProdutoDto>>
{
    /// <summary>
    /// Categoria do produto
    /// </summary>
    /// <example>Lanche</example>
    /// <remarks>
    /// Valores válidos: Lanche, Acompanhamento, Bebida
    /// </remarks>
    public string Categoria { get; set; }

    public ObterProdutosPorCategoriaQuery(string categoria)
    {
        Categoria = categoria;
    }
}