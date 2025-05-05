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
    public class ExclusaoProdutoSteps
    {
        private readonly IMediator _mediator;
        private ProdutoDto? _produto;
        private bool _excluido;

        public ExclusaoProdutoSteps()
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
        [Given(@"um produto já cadastrado no sistema")]
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

        [Given(@"o sistema está pronto para exclusão")]
        public void DadoSistemaProntoParaExclusao()
        {
            // Configuração inicial do sistema
        }

        [When(@"solicito a exclusão do produto")]
        public async Task QuandoExcluirProduto()
        {
            var command = new ExcluirProdutoCommand { Id = _produto.Id };
            await _mediator.Send(command);
            _excluido = true;
        }

        [When(@"tento excluir um produto inexistente")]
        public async Task QuandoExcluirInexistente()
        {
            var command = new ExcluirProdutoCommand { Id = -1 };
            await Assert.ThrowsAsync<ExcecaoNaoEncontrado>(() => _mediator.Send(command));
        }

        [Then(@"o produto deve ser removido do sistema")]
        public async Task EntaoProdutoExcluido()
        {
            var produto = await _mediator.Send(new ObterProdutoQuery(_produto.Id));
            Assert.Null(produto);
            Assert.True(_excluido, "O produto não foi excluído!");
        }
    }
}
