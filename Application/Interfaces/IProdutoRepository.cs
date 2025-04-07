using Domain.Entities;

namespace Application.Interfaces;

public interface IProdutoRepository
{
    Task<Produto> ObterPorIdAsync(string id);
    Task<IEnumerable<Produto>> ObterTodosAsync();
    Task AdicionarAsync(Produto produto);
    Task AtualizarAsync(Produto produto);
    Task ExcluirAsync(Produto produto);
}