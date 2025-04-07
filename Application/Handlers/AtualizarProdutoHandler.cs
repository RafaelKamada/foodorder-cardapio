// Application/Handlers/AtualizarProdutoHandler.cs
using Application.Commands;
using Application.Repositories.Interfaces;
using Domain.Entities;
using MediatR;
using Application.DTOs;
using MongoDB.Bson;

namespace Application.Handlers;

public class AtualizarProdutoHandler : IRequestHandler<AtualizarProdutoCommand, ProdutoDto>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    public AtualizarProdutoHandler(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository)
    {
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
    }

    public async Task<ProdutoDto> Handle(AtualizarProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(request.Id);
        if (produto == null)
            throw new Exception("Produto não encontrado");

        if (request.Nome != null) produto.Nome = request.Nome;
        if (request.Tipo != null) produto.Tipo = request.Tipo.Value;
        if (request.Preco != null) produto.Preco = request.Preco.Value;
        if (request.Descricao != null) produto.Descricao = request.Descricao;
        if (request.TempoPreparo != null) produto.TempoPreparo = request.TempoPreparo.Value;
        if (request.Imagens != null) produto.Imagens = request.Imagens.Select(i => new Imagem
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Data = DateTime.UtcNow,
            ProdutoId = produto.Id
        }).ToList();

        var produtoAtualizado = await _produtoRepository.AtualizarAsync(produto);
        return new ProdutoDto
        {
            Id = produtoAtualizado.Id,
            Nome = produtoAtualizado.Nome,
            Tipo = produtoAtualizado.Tipo,
            Preco = produtoAtualizado.Preco,
            Descricao = produtoAtualizado.Descricao,
            TempoPreparo = produtoAtualizado.TempoPreparo,
            Imagens = produtoAtualizado.Imagens.Select(i => new ImagemDto
            {
                Id = i.Id,
                Data = i.Data,
                ProdutoId = i.ProdutoId
            }).ToList()
        };
    }
}