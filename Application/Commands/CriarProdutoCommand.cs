using MediatR;
using Domain.Enums;
using Application.DTOs;
namespace Application.Commands;

public class CriarProdutoCommand : IRequest<ProdutoDto>
{
    public string Nome { get; set; }
    public string Tipo { get; set; }
    public decimal Preco { get; set; }
    public string Descricao { get; set; }
    public int TempoPreparo { get; set; }
    public List<string> Imagens { get; set; } = new();
}