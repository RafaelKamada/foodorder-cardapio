using Domain.Enums;

namespace Application.DTOs;

public class CategoriaDto
{
    public string Id { get; set; }
    public CategoriaTipo Tipo { get; set; }
    public string Nome => Tipo.ToString();
}