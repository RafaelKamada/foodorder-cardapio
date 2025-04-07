using Domain.Enums;
using Application.Repositories.Interfaces;
using Domain.Entities;
using MongoDB.Driver;
using Infrastructure.Configurations;

namespace Infrastructure.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly IMongoCollection<Produto> _produtos;

    public ProdutoRepository(MongoDbContext context)
    {
        _produtos = context.GetCollection<Produto>("produtos");
    }

    public async Task<Produto> ObterPorIdAsync(string id)
    {
        return await _produtos.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Produto>> ObterTodosAsync()
    {
        return await _produtos.Find(_ => true).ToListAsync();
    }

    public async Task<List<Produto>> ObterPorCategoriaAsync(CategoriaTipo categoria)
    {
        return await _produtos.Find(p => p.Tipo == categoria).ToListAsync();
    }

    public async Task<Produto> CriarAsync(Produto produto)
    {
        await _produtos.InsertOneAsync(produto);
        return produto;
    }

    public async Task<Produto> AtualizarAsync(Produto produto)
    {
        await _produtos.ReplaceOneAsync(p => p.Id == produto.Id, produto);
        return produto;
    }

    public async Task<bool> ExcluirAsync(string id)
    {
        var result = await _produtos.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }
}