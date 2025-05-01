using Application.Commands;
using Application.DTOs;
using Application.Handlers;
using Application.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using Tests.Repositories;

namespace Tests.BDD.Steps
{
    [Binding]
    public class ListagemProdutosSteps
    {
        private readonly IMediator _mediator;
        private List<ProdutoDto>? _produtos;
        private ProdutoDto? _produto;


        public ListagemProdutosSteps()
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
                cfg.RegisterServicesFromAssembly(typeof(ObterProdutosPorCategoriaQuery).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ObterProdutosPorCategoriaHandler).Assembly);
            });
            services.AddScoped(typeof(IRequestHandler<ObterProdutosPorCategoriaQuery, List<ProdutoDto>>), typeof(ObterProdutosPorCategoriaHandler));

            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Given(@"existem produtos cadastrados no sistema")]
        public async Task DadoProdutosCadastrados()
        {
            // Criar produtos de teste
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

        [When(@"solicito a listagem de produtos")]
        public async Task QuandoListarProdutos()
        {
            var query = new ObterTodosProdutosQuery();
            _produtos = await _mediator.Send(query);
        }

        [When(@"solicito a listagem de produtos por categoria")]
        public async Task QuandoListarPorCategoria()
        {
            _produtos = await _mediator.Send(new ObterProdutosPorCategoriaQuery("Lanche"));
        }

        [Then(@"o sistema deve retornar a lista completa de produtos")]
        public void EntaoListaCompleta()
        {
            Assert.NotNull(_produtos);
            Assert.NotEmpty(_produtos);
            Assert.Contains(_produtos, p => p.Nome == "Hambuguer");
        }

        [Then(@"o sistema deve retornar apenas os produtos daquela categoria")]
        public void EntaoProdutosPorCategoria()
        {
            Assert.NotNull(_produtos);
            Assert.All(_produtos, p => Assert.Equal("Lanche", p.Tipo));
        }
    }
}
