using Domain.Entities;

namespace Application.Interfaces;

public interface ICategoriaRepository
{
    Task<Categoria> ObterPorIdAsync(string id);
    Task<IEnumerable<Categoria>> ObterTodosAsync();
    Task AdicionarAsync(Categoria categoria);
    Task AtualizarAsync(Categoria categoria);
    Task ExcluirAsync(Categoria categoria);
}