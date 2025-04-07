// Infrastructure/Repositories/ImagemRepository.cs
using Application.Repositories.Interfaces;
using Domain.Entities;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class ImagemRepository : IImagemRepository
{
    private readonly IMongoCollection<Imagem> _imagens;

    public ImagemRepository(IMongoDatabase database)
    {
        _imagens = database.GetCollection<Imagem>("imagens");
    }

    public async Task<Imagem> ObterPorIdAsync(string id)
    {
        return await _imagens.Find(i => i.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Imagem>> ObterPorProdutoIdAsync(string produtoId)
    {
        return await _imagens.Find(i => i.ProdutoId == produtoId).ToListAsync();
    }

    public async Task<Imagem> CriarAsync(Imagem imagem)
    {
        await _imagens.InsertOneAsync(imagem);
        return imagem;
    }

    public async Task<bool> ExcluirAsync(string id)
    {
        var result = await _imagens.DeleteOneAsync(i => i.Id == id);
        return result.DeletedCount > 0;
    }
}