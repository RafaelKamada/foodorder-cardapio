using Domain.Entities;

namespace Application.Repositories.Interfaces;

public interface ICategoriaRepository
{
    Task<Categoria> ObterPorIdAsync(string id);
    Task<List<Categoria>> ObterTodosAsync();
    Task<Categoria> CriarAsync(Categoria categoria);
    Task<Categoria> AtualizarAsync(Categoria categoria);
    Task<bool> ExcluirAsync(string id);
}