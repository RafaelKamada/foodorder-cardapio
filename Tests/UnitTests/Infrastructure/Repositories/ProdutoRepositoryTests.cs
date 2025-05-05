using Domain.Entities;
using Domain.Enums;
using Infrastructure.Configurations;
using Infrastructure.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Moq;
using MongoDB.Driver.Linq;

namespace Tests.UnitTests.Infrastructure.Repositories
{
    public class ProdutoRepositoryTests
    {

        private readonly Mock<IMongoDbContext> _contextMock;
        private readonly Mock<IMongoCollection<Produto>> _produtosCollectionMock;
        private readonly Mock<IAsyncCursor<Produto>> _cursorMock;
        private readonly ProdutoRepository _repository;
        private readonly List<Produto> _produtos;

        public ProdutoRepositoryTests()
        {
            _contextMock = new Mock<IMongoDbContext>();
            _produtosCollectionMock = new Mock<IMongoCollection<Produto>>();
            _cursorMock = new Mock<IAsyncCursor<Produto>>();

            _produtos = new List<Produto>
            {
                new Produto
                {
                    IdSequencial = 1,
                    Nome = "Hamburguer",
                    Preco = 25.0M,
                    Descricao = "Hamburguer artesanal",
                    Tipo = CategoriaTipo.Lanche.ToString(),
                    TempoPreparo = 15,
                    Imagens = new List<Imagem>
                    {
                        new Imagem
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            Data = DateTime.UtcNow,
                            ProdutoId = "1",
                            Base64Data = "base64data"
                        }
                    }
                },
                new Produto
                {
                    IdSequencial = 2,
                    Nome = "Pizza",
                    Preco = 50.0M,
                    Descricao = "Pizza de calabresa",
                    Tipo = CategoriaTipo.Lanche.ToString(),
                    TempoPreparo = 30,
                    Imagens = new List<Imagem>()
                }
            };

            SetupMocks(_produtos);
            _repository = new ProdutoRepository(_contextMock.Object);
        }

        private void SetupMocks(List<Produto> produtos)
        {
            _cursorMock.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            _cursorMock.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            _produtosCollectionMock.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Produto>>(),
                It.IsAny<FindOptions<Produto, Produto>>(),
                It.IsAny<CancellationToken>()))
            .Returns<FilterDefinition<Produto>, FindOptions<Produto, Produto>, CancellationToken>(
                (filter, options, token) =>
                {
                    var serializerRegistry = BsonSerializer.SerializerRegistry;
                    var documentSerializer = serializerRegistry.GetSerializer<Produto>();
                    var filterDoc = filter.Render(documentSerializer, serializerRegistry);

                    var categoriaFiltro = filterDoc.GetValue("Tipo", null)?.AsString;

                    var produtosFiltrados = categoriaFiltro != null
                        ? _produtos.Where(p => p.Tipo == categoriaFiltro).ToList()
                        : _produtos;

                    var mockCursor = new Mock<IAsyncCursor<Produto>>();
                    mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>()))
                        .Returns(true).Returns(false);
                    mockCursor.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true).ReturnsAsync(false);
                    mockCursor.Setup(x => x.Current).Returns(produtosFiltrados);

                    return Task.FromResult(mockCursor.Object);
                });


            _contextMock.Setup(c => c.GetCollection<Produto>("produtos"))
                .Returns(_produtosCollectionMock.Object);
        }


        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarProdutoQuandoEncontrado()
        {
            // Act
            var resultado = await _repository.ObterPorIdAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.IdSequencial);
            Assert.Equal("Hamburguer", resultado.Nome);
        }

        [Fact]
        public async Task ObterPorIdsAsync_DeveRetornarProdutosCorrespondentes()
        {
            // Act
            var ids = new List<int> { 1, 2 };
            var resultados = await _repository.ObterPorIdsAsync(ids);

            // Assert
            Assert.NotNull(resultados);
            Assert.Equal(2, resultados.Count);
            Assert.Contains(resultados, p => p.IdSequencial == 1);
            Assert.Contains(resultados, p => p.IdSequencial == 2);
        }

        [Fact]
        public async Task ObterTodosAsync_DeveRetornarTodosProdutos()
        {
            // Act
            var resultados = await _repository.ObterTodosAsync();

            // Assert
            Assert.NotNull(resultados);
            Assert.Equal(2, resultados.Count);
        }

        [Fact]
        public async Task ObterPorCategoriaAsync_DeveRetornarProdutosDaCategoria()
        {
            // Arrange
            var categoria = CategoriaTipo.Lanche.ToString();

            var produtoEsperado = new Produto
            {
                IdSequencial = 1,
                Nome = "Hamburguer",
                Preco = 25.0M,
                Descricao = "Hamburguer artesanal",
                Tipo = categoria,
                TempoPreparo = 15,
                Imagens = new List<Imagem>
        {
            new Imagem
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Data = DateTime.UtcNow,
                ProdutoId = "1",
                Base64Data = "base64data"
            }
        }
            };

            var outroProduto = new Produto
            {
                IdSequencial = 2,
                Nome = "Pizza",
                Preco = 30.0M,
                Descricao = "Pizza de calabresa",
                Tipo = "Pizza",
                TempoPreparo = 20,
                Imagens = new List<Imagem>()
            };

            var todosProdutos = new List<Produto> { produtoEsperado, outroProduto };
            var retornoEsperado = new List<Produto> { produtoEsperado };

            var cursorMock = new Mock<IAsyncCursor<Produto>>();
            cursorMock.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                      .Returns(true)
                      .Returns(false);
            cursorMock.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true)
                      .ReturnsAsync(false);
            cursorMock.Setup(c => c.Current).Returns(retornoEsperado);

            var collectionMock = new Mock<IMongoCollection<Produto>>();
            FilterDefinition<Produto> capturado = null;

            collectionMock
            .Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Produto>>(),
                It.IsAny<FindOptions<Produto, Produto>>(),
                It.IsAny<CancellationToken>()))
            .Callback<FilterDefinition<Produto>, FindOptions<Produto, Produto>, CancellationToken>(
                (filter, _, _) => capturado = filter)
            .ReturnsAsync(cursorMock.Object);

            var contextMock = new Mock<IMongoDbContext>();
            contextMock.Setup(c => c.GetCollection<Produto>("produtos")).Returns(collectionMock.Object);

            var repository = new ProdutoRepository(contextMock.Object);

            // Act
            var resultado = await repository.ObterPorCategoriaAsync(CategoriaTipo.Lanche);

            // Assert
            Assert.Single(resultado);
            Assert.Equal(produtoEsperado.Nome, resultado[0].Nome);
        }

        [Fact]
        public async Task CriarAsync_DeveAtualizarSequenciaQuandoExistir()
        {
            // Arrange
            var produtoCollectionMock = new Mock<IMongoCollection<Produto>>();
            var sequenceCollectionMock = new Mock<IMongoCollection<Sequence>>();

            var produto = new Produto { Nome = "Produto Teste" };

            var updatedSequence = new Sequence { _id = "Produto", Value = 2 };

            var mongoContextMock = new Mock<IMongoDbContext>();
            mongoContextMock.Setup(x => x.GetCollection<Produto>("produtos")).Returns(produtoCollectionMock.Object);
            mongoContextMock.Setup(x => x.GetCollection<Sequence>("sequences")).Returns(sequenceCollectionMock.Object);

            sequenceCollectionMock.Setup(x =>
                x.FindOneAndUpdateAsync(
                    It.IsAny<FilterDefinition<Sequence>>(),
                    It.IsAny<UpdateDefinition<Sequence>>(),
                    It.IsAny<FindOneAndUpdateOptions<Sequence>>(),
                    It.IsAny<CancellationToken>())
            ).ReturnsAsync(updatedSequence);

            produtoCollectionMock
                .Setup(x => x.InsertOneAsync(It.IsAny<Produto>(), null, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var repo = new ProdutoRepository(mongoContextMock.Object);

            // Act
            var result = await repo.CriarAsync(produto);

            // Assert
            Assert.Equal(2, result.IdSequencial);
        }

        [Fact]
        public async Task CriarAsync_DeveCriarSequenciaQuandoNaoExistir()
        {
            // Arrange
            var produtoCollectionMock = new Mock<IMongoCollection<Produto>>();
            var sequenceCollectionMock = new Mock<IMongoCollection<Sequence>>();

            var produto = new Produto { Nome = "Produto Teste" };

            var mongoContextMock = new Mock<IMongoDbContext>();
            mongoContextMock.Setup(x => x.GetCollection<Produto>("produtos")).Returns(produtoCollectionMock.Object);
            mongoContextMock.Setup(x => x.GetCollection<Sequence>("sequences")).Returns(sequenceCollectionMock.Object);

            sequenceCollectionMock.Setup(x =>
                x.FindOneAndUpdateAsync(
                    It.IsAny<FilterDefinition<Sequence>>(),
                    It.IsAny<UpdateDefinition<Sequence>>(),
                    It.IsAny<FindOneAndUpdateOptions<Sequence>>(),
                    It.IsAny<CancellationToken>())
            ).ReturnsAsync((Sequence)null); 

            sequenceCollectionMock
                .Setup(x => x.InsertOneAsync(It.Is<Sequence>(s => s._id == "Produto" && s.Value == 1), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            produtoCollectionMock
                .Setup(x => x.InsertOneAsync(It.IsAny<Produto>(), null, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var repo = new ProdutoRepository(mongoContextMock.Object);

            // Act
            var result = await repo.CriarAsync(produto);

            // Assert
            Assert.Equal(1, result.IdSequencial);
            sequenceCollectionMock.Verify(); // Garante que InsertOneAsync foi chamado para a sequência
            produtoCollectionMock.Verify();  // Garante que InsertOneAsync foi chamado para o produto
        }

        [Fact]
        public async Task CriarAsync_DeveLancarExcecaoQuandoErroNoBanco()
        {
            // Arrange
            var sequenceCollectionMock = new Mock<IMongoCollection<Sequence>>();
            var produtoCollectionMock = new Mock<IMongoCollection<Produto>>();

            sequenceCollectionMock.Setup(x => x.FindOneAndUpdateAsync(
                It.IsAny<FilterDefinition<Sequence>>(),
                It.IsAny<UpdateDefinition<Sequence>>(),
                It.IsAny<FindOneAndUpdateOptions<Sequence>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro no banco"));

            var contextMock = new Mock<IMongoDbContext>();
            contextMock.Setup(c => c.GetCollection<Produto>("produtos")).Returns(produtoCollectionMock.Object);
            contextMock.Setup(c => c.GetCollection<Sequence>("sequences")).Returns(sequenceCollectionMock.Object);

            var repository = new ProdutoRepository(contextMock.Object);
            var produto = new Produto { IdSequencial = 0 };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => repository.CriarAsync(produto));
            Assert.Equal("Erro no banco", ex.Message);
        }

        //[Fact]
        //public async Task AtualizarAsync_DeveAtualizarProdutoQuandoExistir()
        //{
        //    // Arrange
        //    var produtoExistente = new Produto { IdSequencial = 1, Nome = "Antigo" };
        //    var produtoAtualizado = new Produto
        //    {
        //        IdSequencial = 1,
        //        Nome = "Novo",
        //        Preco = 9.99m,
        //        Descricao = "Atualizado",
        //        Tipo = "CategoriaA",
        //        TempoPreparo = 10,
        //        Imagens = new List<Imagem>
        //{
        //    new Imagem
        //    {
        //        Id = ObjectId.GenerateNewId().ToString(),
        //        Data = DateTime.UtcNow,
        //        ProdutoId = "1",
        //        Base64Data = "base64data"
        //    }
        //}
        //    };

        //    var produtoCollectionMock = new Mock<IMongoCollection<Produto>>();
        //    var sequenceCollectionMock = new Mock<IMongoCollection<Sequence>>();

        //    // Simula o comportamento do Find com filtro, que deve retornar o produto existente.
        //    produtoCollectionMock
        //        .Setup(x => x.Find(It.IsAny<FilterDefinition<Produto>>(), It.IsAny<FindOptions>()))
        //        .Returns(Mock.Of<IFindFluent<Produto, Produto>>);

        //    // Simula a chamada ReplaceOneAsync para atualizar o produto.
        //    produtoCollectionMock
        //        .Setup(x => x.ReplaceOneAsync(
        //            It.IsAny<FilterDefinition<Produto>>(),
        //            It.IsAny<Produto>(),
        //            It.IsAny<ReplaceOptions>(),
        //            It.IsAny<CancellationToken>()))
        //        .Returns(Task.FromResult<ReplaceOneResult>(new ReplaceOneResult.Acknowledged(1, 1, null)));


        //    var contextMock = new Mock<IMongoDbContext>();
        //    contextMock.Setup(x => x.GetCollection<Produto>("produtos")).Returns(produtoCollectionMock.Object);
        //    contextMock.Setup(x => x.GetCollection<Sequence>("sequences")).Returns(sequenceCollectionMock.Object);

        //    var repo = new ProdutoRepository(contextMock.Object);

        //    // Act
        //    var result = await repo.AtualizarAsync(produtoAtualizado);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal("Novo", result.Nome);
        //}


        [Fact]
        public async Task ExcluirAsync_DeveRetornarTrueQuandoProdutoForExcluido()
        {
            // Arrange
            var produtoCollectionMock = new Mock<IMongoCollection<Produto>>();
            var sequenceCollectionMock = new Mock<IMongoCollection<Sequence>>();

            produtoCollectionMock
                .Setup(x => x.DeleteOneAsync(
                    It.IsAny<FilterDefinition<Produto>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DeleteResult.Acknowledged(1));

            var contextMock = new Mock<IMongoDbContext>();
            contextMock.Setup(x => x.GetCollection<Produto>("produtos")).Returns(produtoCollectionMock.Object);
            contextMock.Setup(x => x.GetCollection<Sequence>("sequences")).Returns(sequenceCollectionMock.Object);

            var repo = new ProdutoRepository(contextMock.Object);

            // Act
            var resultado = await repo.ExcluirAsync(1);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public async Task ExcluirAsync_DeveRetornarFalseQuandoProdutoNaoForEncontrado()
        {
            // Arrange
            var produtoCollectionMock = new Mock<IMongoCollection<Produto>>();
            var sequenceCollectionMock = new Mock<IMongoCollection<Sequence>>();

            produtoCollectionMock
                .Setup(x => x.DeleteOneAsync(
                    It.IsAny<FilterDefinition<Produto>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DeleteResult.Acknowledged(0));

            var contextMock = new Mock<IMongoDbContext>();
            contextMock.Setup(x => x.GetCollection<Produto>("produtos")).Returns(produtoCollectionMock.Object);
            contextMock.Setup(x => x.GetCollection<Sequence>("sequences")).Returns(sequenceCollectionMock.Object);

            var repo = new ProdutoRepository(contextMock.Object);

            // Act
            var resultado = await repo.ExcluirAsync(999);

            // Assert
            Assert.False(resultado);
        }

    }
}