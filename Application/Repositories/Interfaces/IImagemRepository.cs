using Domain.Entities;

namespace Application.Repositories.Interfaces;

public interface IImagemRepository
{
    Task<Imagem> ObterPorIdAsync(string id);
    Task<List<Imagem>> ObterPorProdutoIdAsync(string produtoId);
    Task<Imagem> CriarAsync(Imagem imagem);
    Task<bool> ExcluirAsync(string id);
}