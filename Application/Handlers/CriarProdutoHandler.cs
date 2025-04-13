using Application.Commands;
using Application.Repositories.Interfaces;
using Domain.Entities;
using MediatR;
using Application.DTOs;
using MongoDB.Bson;
using Domain.Exceptions;

namespace Application.Handlers;

public class CriarProdutoHandler : IRequestHandler<CriarProdutoCommand, ProdutoDto>
{
    private readonly IProdutoRepository _produtoRepository;
    //private readonly ICategoriaRepository _categoriaRepository;

    public CriarProdutoHandler(IProdutoRepository produtoRepository/*, ICategoriaRepository categoriaRepository*/)
    {
        _produtoRepository = produtoRepository;
        //_categoriaRepository = categoriaRepository;
    }

    public async Task<ProdutoDto> Handle(CriarProdutoCommand request, CancellationToken cancellationToken)
    {
        var produto = new Produto
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Nome = request.Nome,
            Tipo = request.Tipo,
            Preco = request.Preco,
            Descricao = request.Descricao,
            TempoPreparo = request.TempoPreparo,
            Imagens = request.Imagens.Select(i => new Imagem
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Data = DateTime.UtcNow,
                ProdutoId = ObjectId.GenerateNewId().ToString(),
                Base64Data = i.ToString()
            }).ToList()
        };

        var produtoCriado = await _produtoRepository.CriarAsync(produto);
        
        // Atualizar ProdutoId das imagens após o produto ser criado
        foreach (var imagem in produtoCriado.Imagens)
        {
            imagem.ProdutoId = produtoCriado.Id;
        }

        var result = await _produtoRepository.AtualizarAsync(produtoCriado);

        if (result == null)
        {
            throw new ExcecaoNaoEncontrado($"Produto com ID {produto.IdSequencial} não encontrado!");
        }

        return new ProdutoDto
        {
            Id = produtoCriado.IdSequencial,
            Nome = produtoCriado.Nome,
            Tipo = produtoCriado.Tipo,
            Preco = produtoCriado.Preco,
            Descricao = produtoCriado.Descricao,
            TempoPreparo = produtoCriado.TempoPreparo,
            Imagens = produtoCriado.Imagens.Select(i => new ImagemDto
            {
                Id = i.Id,
                Data = i.Data,
                ProdutoId = i.ProdutoId                
            }).ToList()
        };
    }
}