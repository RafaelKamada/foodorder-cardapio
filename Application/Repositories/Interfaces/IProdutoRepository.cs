using Domain.Entities;
using Domain.Enums;

namespace Application.Repositories.Interfaces;

public interface IProdutoRepository
{
    Task<Produto> ObterPorIdAsync(int id);
    Task<List<Produto>> ObterTodosAsync();
    Task<List<Produto>> ObterPorCategoriaAsync(CategoriaTipo categoria);
    Task<Produto> CriarAsync(Produto produto);
    Task<Produto> AtualizarAsync(Produto produto);
    Task<bool> ExcluirAsync(int id);
}