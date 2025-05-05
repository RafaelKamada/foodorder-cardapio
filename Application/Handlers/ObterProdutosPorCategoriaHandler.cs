using Application.Queries;
using Application.Repositories.Interfaces;
using Application.DTOs;
using MediatR;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.Handlers
{
    public class ObterProdutosPorCategoriaHandler : IRequestHandler<ObterProdutosPorCategoriaQuery, List<ProdutoDto>>
    {
        private readonly IProdutoRepository _produtoRepository;

        public ObterProdutosPorCategoriaHandler(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<List<ProdutoDto>> Handle(ObterProdutosPorCategoriaQuery request, CancellationToken cancellationToken)
        {

            if (!Enum.TryParse<CategoriaTipo>(request.Categoria, true, out CategoriaTipo categoria))
            {
                var categoriasValidas = string.Join(", ", Enum.GetNames<CategoriaTipo>());
                throw new ExcecaoValidacao($"Categoria inv�lida. Por favor, use uma das seguintes categorias: {categoriasValidas}");
            }

            var produtos = await _produtoRepository.ObterPorCategoriaAsync(categoria);

            return produtos.Select(p => new ProdutoDto
            {
                Id = p.IdSequencial,
                Nome = p.Nome,
                Tipo = p.Tipo,
                Preco = p.Preco,
                Descricao = p.Descricao,
                TempoPreparo = p.TempoPreparo,
                Imagens = p.Imagens.Select(i => new ImagemDto
                {
                    Id = i.Id,
                    Data = i.Data,
                    ProdutoId = i.ProdutoId
                }).ToList()
            }).ToList();            
        }
    }
}