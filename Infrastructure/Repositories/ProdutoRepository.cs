using Domain.Enums;
using Application.Repositories.Interfaces;
using Domain.Entities;
using MongoDB.Driver;
using Infrastructure.Configurations;

namespace Infrastructure.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly IMongoCollection<Produto> _produtos;
    private readonly IMongoCollection<Sequence> _sequences;

    public ProdutoRepository(MongoDbContext context)
    {
        _produtos = context.GetCollection<Produto>("produtos");
        _sequences = context.GetCollection<Sequence>("sequences");
    }

    public async Task<Produto> ObterPorIdAsync(int id)
    {
        return await _produtos.Find(p => p.IdSequencial == id).FirstOrDefaultAsync();
    }

    public async Task<List<Produto>> ObterTodosAsync()
    {
        return await _produtos.Find(_ => true).ToListAsync();
    }

    public async Task<List<Produto>> ObterPorCategoriaAsync(CategoriaTipo categoria)
    {
        return await _produtos.Find(p => p.Tipo == categoria.ToString()).ToListAsync();
    }

    public async Task<Produto> CriarAsync(Produto produto)
    {
        // Busca e atualiza a sequência de forma atômica
        var sequence = await _sequences.FindOneAndUpdateAsync(
            s => s._id == "Produto",
            Builders<Sequence>.Update.Inc(x => x.Value, 1),
            new FindOneAndUpdateOptions<Sequence> { ReturnDocument = ReturnDocument.After }
        );

        // Se não existir, cria a sequência
        if (sequence == null)
        {
            sequence = new Sequence { _id = "Produto", Value = 1 };
            await _sequences.InsertOneAsync(sequence);
        }

        produto.IdSequencial = sequence.Value;
        await _produtos.InsertOneAsync(produto);
        return produto;
    }

    public async Task<Produto> AtualizarAsync(Produto produto)
    {
        // Primeiro, verifique se o produto existe
        var produtoExistente = await _produtos.Find(p => p.IdSequencial == produto.IdSequencial).FirstOrDefaultAsync();

        if (produtoExistente == null)
        {
            return null;
        }

        var updateDefinition = Builders<Produto>.Update
            .Set(p => p.Nome, produto.Nome)
            .Set(p => p.Preco, produto.Preco)
            .Set(p => p.Descricao, produto.Descricao)
            .Set(p => p.Tipo, produto.Tipo)
            .Set(p => p.TempoPreparo, produto.TempoPreparo)
            .Set(p => p.Imagens, produto.Imagens);

        var result = await _produtos.UpdateOneAsync(
            p => p.IdSequencial == produto.IdSequencial,
            updateDefinition,
            new UpdateOptions { IsUpsert = false }
        );

        if (result.ModifiedCount == 0)
        {
            return null;
        }

        return await _produtos.Find(p => p.IdSequencial == produto.IdSequencial).FirstOrDefaultAsync();
        //await _produtos.ReplaceOneAsync(p => p.Id == produto.Id, produto);
        //return produto;
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        var result = await _produtos.DeleteOneAsync(p => p.IdSequencial == id);
        return result.DeletedCount > 0;
    }
}