using Domain.Entities;
using Infrastructure.Configurations;
using Infrastructure.Repositories;
using Moq;
using MongoDB.Driver;
using FluentAssertions;

namespace Tests.UnitTests.Infrastructure.Repositories
{
    public class CategoriaRepositoryTests
    {
        private readonly Mock<IMongoDbContext> _contextMock;
        private readonly Mock<IMongoCollection<Categoria>> _categoriaCollectionMock;
        private readonly Mock<IAsyncCursor<Categoria>> _cursorMock;
        private readonly CategoriaRepository _repository;
        private readonly List<Categoria> _categorias;

        public CategoriaRepositoryTests()
        {
            _contextMock = new Mock<IMongoDbContext>();
            _categoriaCollectionMock = new Mock<IMongoCollection<Categoria>>();
            _cursorMock = new Mock<IAsyncCursor<Categoria>>();

            _categorias = new List<Categoria>
            {
                new Categoria { Id = "1", Nome = "Lanche", Tipo = Domain.Enums.CategoriaTipo.Lanche },
                new Categoria { Id = "2", Nome = "Acompanhamento", Tipo = Domain.Enums.CategoriaTipo.Acompanhamento },
                new Categoria { Id = "3", Nome = "Bebida", Tipo = Domain.Enums.CategoriaTipo.Bebida }
            };

            _cursorMock.Setup(_ => _.Current).Returns(() => _categorias);
            _cursorMock
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            _cursorMock
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            _categoriaCollectionMock
                .Setup(m => m.FindAsync(
                    It.IsAny<FilterDefinition<Categoria>>(),
                    It.IsAny<FindOptions<Categoria, Categoria>>(),
                    It.IsAny<CancellationToken>())
                )
                .ReturnsAsync(_cursorMock.Object);

            _categoriaCollectionMock
                .Setup(c => c.ReplaceOneAsync(
                    It.IsAny<FilterDefinition<Categoria>>(),
                    It.IsAny<Categoria>(),
                    It.IsAny<ReplaceOptions>(),
                    It.IsAny<CancellationToken>()))
                .Callback<FilterDefinition<Categoria>, Categoria, ReplaceOptions, CancellationToken>((filter, categoria, _, _) =>
                {
                    var index = _categorias.FindIndex(c => c.Id == categoria.Id);
                    if (index != -1)
                    {
                        _categorias[index] = categoria;
                    }
                })
                .ReturnsAsync(new ReplaceOneResult.Acknowledged(1, 1, null));

            _categoriaCollectionMock
                .Setup(c => c.DeleteOneAsync(
                    It.IsAny<FilterDefinition<Categoria>>(),
                    It.IsAny<CancellationToken>()))
                .Callback<FilterDefinition<Categoria>, CancellationToken>((filter, _) =>
                {
                    var field = typeof(FilterDefinition<Categoria>)
                                    .GetField("_document", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                    var value = field?.GetValue(filter) as MongoDB.Bson.BsonDocument;
                    var id = value?.GetValue("_id").AsString;

                    _categorias.RemoveAll(c => c.Id == id);
                })
                .ReturnsAsync(new DeleteResult.Acknowledged(1));

            _contextMock.Setup(c => c.GetCollection<Categoria>("categorias"))
                        .Returns(_categoriaCollectionMock.Object);


            _repository = new CategoriaRepository(_contextMock.Object);
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarCategoria_QuandoEncontrada()
        {
            // Arrange
            var categoriaEsperada = _categorias[0];
            _cursorMock.Setup(x => x.Current).Returns(new List<Categoria> { categoriaEsperada });
            _cursorMock.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(true).ReturnsAsync(false);

            _categoriaCollectionMock.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Categoria>>(),
                It.IsAny<FindOptions<Categoria, Categoria>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(_cursorMock.Object);

            // Act
            var resultado = await _repository.ObterPorIdAsync("1");

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("1", resultado.Id);
            Assert.Equal("Lanche", resultado.Nome);
        }

        [Fact]
        public async Task ObterTodosAsync_DeveRetornarTodasCategorias()
        {
            // Arrange
            _cursorMock.Setup(x => x.Current).Returns(_categorias);
            _cursorMock.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(true).ReturnsAsync(false);

            _categoriaCollectionMock.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Categoria>>(),
                It.IsAny<FindOptions<Categoria, Categoria>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(_cursorMock.Object);

            // Act
            var resultado = await _repository.ObterTodosAsync();

            // Assert
            Assert.Equal(3, resultado.Count);
        }

        [Fact]
        public async Task CriarAsync_DeveInserirCategoriaERetornarObjetoCriado()
        {
            // Arrange
            var novaCategoria = new Categoria { Id = "3", Nome = "Acompanhamento", Tipo = Domain.Enums.CategoriaTipo.Acompanhamento };
            _categoriaCollectionMock.Setup(c =>
                c.InsertOneAsync(novaCategoria, null, default)).Returns(Task.CompletedTask);

            // Act
            var resultado = await _repository.CriarAsync(novaCategoria);

            // Assert
            Assert.Equal("3", resultado.Id);
            Assert.Equal("Acompanhamento", resultado.Nome);
        }

        [Fact]
        public async Task AtualizarAsync_DeveAtualizarCategoriaExistente()
        {
            // Arrange
            var categoriaAtualizada = new Categoria { Id = "3", Nome = "Bebida", Tipo = Domain.Enums.CategoriaTipo.Bebida };

            // Act
            var categoria = await _repository.AtualizarAsync(categoriaAtualizada);

            // Assert
            categoria.Should().NotBeNull();
            categoria.Id.Should().Be("3");
            categoria.Nome.Should().Be("Bebida");

            _cursorMock.Setup(x => x.Current).Returns(new List<Categoria> { categoriaAtualizada });
            _cursorMock.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(true).ReturnsAsync(false);

            var categoriaNoRepo = await _repository.ObterPorIdAsync("3");
            categoriaNoRepo.Nome.Should().Be("Bebida");
        }


        [Fact]
        public async Task ExcluirAsync_ComIdExistente_DeveRemoverCategoria()
        {
            // Act
            var resultado = await _repository.ExcluirAsync("1");

            // Assert
            resultado.Should().BeTrue();

            _cursorMock.Setup(x => x.Current).Returns(new List<Categoria>());
            _cursorMock.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(true).ReturnsAsync(false);

            var categoria = await _repository.ObterPorIdAsync("1");
            categoria.Should().BeNull();
        }


    }
}
