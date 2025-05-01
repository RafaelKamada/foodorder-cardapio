using Application.Commands;
using Application.DTOs;
using Application.Handlers;
using Application.Queries;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using Tests.Repositories;

namespace Tests.BDD.Steps
{
    [Binding]
    public class AtualizacaoProdutoSteps
    {
        private readonly IMediator _mediator;
        private ProdutoDto? _produto;
        private bool _atualizado;

        public AtualizacaoProdutoSteps()
        {
            var services = new ServiceCollection();

            // Configuração dos repositórios
            services.AddScoped<Application.Repositories.Interfaces.IProdutoRepository, ProdutoRepositoryEmMemoria>();

            // Configuração do MediatR
            services.AddMediatR(cfg =>
            {
                // Registra o handler de forma mais explícita
                cfg.RegisterServicesFromAssembly(typeof(ObterTodosProdutosQuery).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ObterTodosProdutosHandler).Assembly);
            });

            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Given(@"um produto ja cadastrado no sistema")]
        public async Task DadoProdutoCadastrado()
        {
            var command = new CriarProdutoCommand
            {
                Nome = "Hambuguer",
                Tipo = "Lanche",
                Preco = 9.99M,
                TempoPreparo = 10,
                Imagens = new List<string> { "data:image/jpeg;base64,..." }
            };

            _produto = await _mediator.Send(command);
        }

        [Given(@"o sistema está pronto para atualização")]
        public void DadoSistemaProntoParaAtualizacao()
        {
            // Configuração inicial do sistema
        }

        [When(@"atualizo as informações do produto")]
        public async Task QuandoAtualizarProduto()
        {
            var command = new AtualizarProdutoCommand
            {
                IdSequencial = _produto.Id,
                Nome = "Hambuguer Atualizado",
                Tipo = "Lanche",
                Preco = 10.99M,
                TempoPreparo = 15
            };

            await _mediator.Send(command);
            _atualizado = true;
        }

        [When(@"tento atualizar um produto inexistente")]
        public async Task QuandoAtualizarInexistente()
        {
            var command = new AtualizarProdutoCommand
            {
                IdSequencial = -1,
                Nome = "Hambuguer",
                Tipo = "Lanche",
                Preco = 9.99M,
                TempoPreparo = 10
            };

            await Assert.ThrowsAsync<ExcecaoNaoEncontrado>(() => _mediator.Send(command));
        }

        [Then(@"o produto deve ser atualizado com sucesso")]
        public async Task EntaoProdutoAtualizado()
        {
            var produtoAtualizado = await _mediator.Send(new ObterProdutoQuery(_produto.Id));
            Assert.NotNull(produtoAtualizado);
            Assert.Equal("Hambuguer Atualizado", produtoAtualizado.Nome);
            Assert.Equal(10.99M, produtoAtualizado.Preco);
            Assert.Equal(15, produtoAtualizado.TempoPreparo);
            Assert.True(_atualizado, "O produto não foi atualizado!");
        }

    }
}
