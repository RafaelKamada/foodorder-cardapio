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
    public class ValidacaoProdutoStep
    {
        private readonly IMediator _mediator;
        private ProdutoDto? _produto;
        private bool _salvoNoBanco;

        public ValidacaoProdutoStep()
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


        [When(@"tento cadastrar um produto com nome vazio")]
        public async Task QuandoTentoCadastrarUmProdutoComNomeVazio()
        {
            var command = new CriarProdutoCommand
            {
                Nome = null,  // Nome vazio
                Tipo = "Lanche",
                Preco = 9.99M,
                TempoPreparo = 10,
                Imagens = new List<string> { "data:image/jpeg;base64,..." }
            };

            await Assert.ThrowsAsync<ExcecaoValidacao>(() => _mediator.Send(command));
        }

        [When(@"tento cadastrar um produto com preco negativo")]
        public async Task QuandoTentoCadastrarUmProdutoComPrecoNegativo()
        {
            var command = new CriarProdutoCommand
            {
                Nome = "Hambuguer",
                Tipo = "Lanche",
                Preco = -1M,  // Preço negativo
                TempoPreparo = 10,
                Imagens = new List<string> { "data:image/jpeg;base64,..." }
            };

            await Assert.ThrowsAsync<ExcecaoValidacao>(() => _mediator.Send(command));
        }

        [When(@"tento cadastrar um produto com tempo de preparo menor que o minimo")]
        public async Task QuandoTentoCadastrarUmProdutoComTempoDePreparoMenorQueOMinimo()
        {
            var command = new CriarProdutoCommand
            {
                Nome = "Hambuguer",
                Tipo = "Lanche",
                Preco = 9.99M,
                TempoPreparo = 2,  // Supondo que o mínimo seja 5 minutos
                Imagens = new List<string> { "data:image/jpeg;base64,..." }
            };

            await Assert.ThrowsAsync<ExcecaoDominio>(() => _mediator.Send(command));
        }

        [Then(@"o sistema deve retornar erro de dominio")]
        public void OSistemaDeveRetornarErroDeDominio()
        {
            // Este é um passo de verificação que já foi realizado no When
        }
    }
}