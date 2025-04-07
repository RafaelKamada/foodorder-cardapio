// Infrastructure/Repositories/CategoriaRepository.cs
using Application.Repositories.Interfaces;
using Domain.Entities;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly IMongoCollection<Categoria> _categorias;

    public CategoriaRepository(IMongoDatabase database)
    {
        _categorias = database.GetCollection<Categoria>("categorias");
    }

    public async Task<Categoria> ObterPorIdAsync(string id)
    {
        return await _categorias.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Categoria>> ObterTodosAsync()
    {
        return await _categorias.Find(_ => true).ToListAsync();
    }

    public async Task<Categoria> CriarAsync(Categoria categoria)
    {
        await _categorias.InsertOneAsync(categoria);
        return categoria;
    }

    public async Task<Categoria> AtualizarAsync(Categoria categoria)
    {
        await _categorias.ReplaceOneAsync(c => c.Id == categoria.Id, categoria);
        return categoria;
    }

    public async Task<bool> ExcluirAsync(string id)
    {
        var result = await _categorias.DeleteOneAsync(c => c.Id == id);
        return result.DeletedCount > 0;
    }
}