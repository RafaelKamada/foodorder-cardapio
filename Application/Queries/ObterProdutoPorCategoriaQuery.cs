using MediatR;
using Application.DTOs;

namespace Application.Queries;

public class ObterProdutoPorCategoriaQuery : IRequest<IEnumerable<ProdutoDto>>
{
    /// <summary>
    /// Categoria do produto
    /// </summary>
    /// <example>Lanche</example>
    /// <remarks>
    /// Valores válidos: Lanche, Acompanhamento, Bebida
    /// </remarks>
    public string Categoria { get; set; }

    public ObterProdutoPorCategoriaQuery(string categoria)
    {
        Categoria = categoria;
    }
}