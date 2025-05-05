using Domain.Entities;

namespace Application.Interfaces;

public interface IImagemRepository
{
    Task<Imagem> ObterPorIdAsync(string id);
    Task<IEnumerable<Imagem>> ObterPorProdutoIdAsync(string produtoId);
    Task AdicionarAsync(Imagem imagem);
    Task AtualizarAsync(Imagem imagem);
    Task ExcluirAsync(Imagem imagem);
}