using Domain.Enums;
using Domain.Entities;

namespace Application.DTOs;

public class ProdutoDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Tipo { get; set; }
    public decimal Preco { get; set; }
    public string Descricao { get; set; }
    public int TempoPreparo { get; set; }
    public List<ImagemDto> Imagens { get; set; } = new();
}
