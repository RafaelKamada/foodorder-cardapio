using Reqnroll;
using MediatR;
using Application.Commands;
using Application.DTOs;
using Application.Queries;
using Microsoft.Extensions.DependencyInjection;
using Application.Handlers;
using Tests.Repositories;

namespace Tests.BDD.Steps
{
    [Binding]
    public class CadastroProdutoSteps
    {
        private readonly IMediator _mediator;
        private ProdutoDto? _produto;
        private bool _salvoNoBanco;

        public CadastroProdutoSteps()
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

            // Verifica se o repositório foi registrado corretamente
            var produtoRepository = provider.GetService<Application.Repositories.Interfaces.IProdutoRepository>();
            if (produtoRepository == null)
            {
                throw new InvalidOperationException("IProdutoRepository não foi registrado corretamente");
            }

            _mediator = provider.GetRequiredService<IMediator>();
        }

        [When(@"eu envio os dados de um novo produto")]
        public async Task QuandoEuEnvioOsDadosDeUmNovoProduto()
        {
            var command = new CriarProdutoCommand
            {
                Nome = "Hambuguer",
                Tipo = "Lanche",
                Preco = 9.99M,
                Descricao = "Hambuguer com queijo",
                TempoPreparo = 10,
                Imagens = new List<string> { "data:image/jpeg;base64,..." }
            };

            var resultado = await _mediator.Send(command);
            Assert.NotNull(resultado);
            _produto = resultado;
            _salvoNoBanco = true;
        }

        [Then(@"o produto e salvo no banco de dados")]
        public async Task EntaoOProdutoESalvoNoBancoDeDados()
        {
            var produto = await _mediator.Send(new ObterProdutoQuery(_produto.Id));
            Assert.NotNull(produto);
            Assert.Equal(_produto.Nome, produto.Nome);
            Assert.Equal("Hambuguer", produto.Nome);
            Assert.Equal("Lanche", produto.Tipo);
            Assert.Equal(9.99M, produto.Preco);
            Assert.Equal(10, produto.TempoPreparo);
            Assert.True(_salvoNoBanco, "O produto não foi salvo no banco!");
        }

    }
}