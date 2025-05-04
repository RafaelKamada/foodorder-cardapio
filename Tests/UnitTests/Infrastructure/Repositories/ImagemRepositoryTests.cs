using Domain.Entities;
using Infrastructure.Repositories;
using Infrastructure.Configurations;
using Moq;
using MongoDB.Driver;
using FluentAssertions;

namespace Tests.UnitTests.Infrastructure.Repositories
{
    public class ImagemRepositoryTests
    {
        private readonly Mock<IMongoDbContext> _contextMock;
        private readonly Mock<IMongoCollection<Imagem>> _imagemCollectionMock;
        private readonly Mock<IAsyncCursor<Imagem>> _cursorMock;
        private readonly ImagemRepository _repository;
        private readonly List<Imagem> _imagens;

        public ImagemRepositoryTests()
        {
            _contextMock = new Mock<IMongoDbContext>();
            _imagemCollectionMock = new Mock<IMongoCollection<Imagem>>();
            _cursorMock = new Mock<IAsyncCursor<Imagem>>();

            _imagens = new List<Imagem>
            {
                new Imagem
                {
                    Id = "1",
                    ProdutoId = "100",
                    Base64Data = "base64data1",
                    Data = DateTime.UtcNow
                },
                new Imagem
                {
                    Id = "2",
                    ProdutoId = "101",
                    Base64Data = "base64data2",
                    Data = DateTime.UtcNow
                }
            };

            _cursorMock.Setup(_ => _.Current).Returns(() => _imagens);
            _cursorMock.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(true).ReturnsAsync(false);

            _imagemCollectionMock
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<Imagem>>(),
                    It.IsAny<FindOptions<Imagem, Imagem>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_cursorMock.Object);

            _imagemCollectionMock
                .Setup(c => c.DeleteOneAsync(
                    It.IsAny<FilterDefinition<Imagem>>(),
                    It.IsAny<CancellationToken>()))
                .Callback<FilterDefinition<Imagem>, CancellationToken>((filter, _) =>
                {
                    var field = typeof(FilterDefinition<Imagem>)
                        .GetField("_document", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                    var value = field?.GetValue(filter) as MongoDB.Bson.BsonDocument;
                    var id = value?.GetValue("_id").AsString;

                    _imagens.RemoveAll(i => i.Id == id);
                })
                .ReturnsAsync(new DeleteResult.Acknowledged(1));

            _contextMock.Setup(c => c.GetCollection<Imagem>("imagens"))
                        .Returns(_imagemCollectionMock.Object);

            _repository = new ImagemRepository(_contextMock.Object);
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarImagem_QuandoExistente()
        {
            var imagemEsperada = _imagens[0];

            _cursorMock.Setup(x => x.Current).Returns(new List<Imagem> { imagemEsperada });

            var resultado = await _repository.ObterPorIdAsync("1");

            resultado.Should().NotBeNull();
            resultado.Id.Should().Be("1");
        }

        [Fact]
        public async Task ObterPorProdutoIdAsync_DeveRetornarImagensRelacionadas()
        {
            var produtoId = "100";
            var imagensEsperadas = _imagens.Where(i => i.ProdutoId == produtoId).ToList();

            _cursorMock.Setup(x => x.Current).Returns(imagensEsperadas);

            var resultado = await _repository.ObterPorProdutoIdAsync(produtoId);

            resultado.Should().NotBeNullOrEmpty();
            resultado.All(i => i.ProdutoId == produtoId).Should().BeTrue();
        }

        [Fact]
        public async Task CriarAsync_DeveAdicionarImagem()
        {
            var novaImagem = new Imagem
            {
                Id = "3",
                ProdutoId = "102",
                Base64Data = "base64data3",
                Data = DateTime.UtcNow
            };

            _imagemCollectionMock.Setup(c => c.InsertOneAsync(novaImagem, null, default))
                                 .Returns(Task.CompletedTask)
                                 .Callback(() => _imagens.Add(novaImagem));

            var resultado = await _repository.CriarAsync(novaImagem);

            resultado.Should().BeEquivalentTo(novaImagem);
            _imagens.Should().Contain(novaImagem);
        }

        [Fact]
        public async Task ExcluirAsync_DeveRemoverImagem()
        {
            // Arrange
            var imagemId = "1";  // ID da imagem que será removida

            var imagemAExcluir = new Imagem
            {
                Id = imagemId,
                ProdutoId = "100",
                Base64Data = "base64data1",
                Data = DateTime.UtcNow
            };

            _imagens.Add(imagemAExcluir);  

            _imagemCollectionMock
                .Setup(c => c.DeleteOneAsync(
                    It.IsAny<FilterDefinition<Imagem>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DeleteResult.Acknowledged(1)) 
                .Callback<FilterDefinition<Imagem>, CancellationToken>((filter, _) =>
                {
                    _imagens.RemoveAll(i => i.Id == imagemId);
                });

            // Act
            var resultado = await _repository.ExcluirAsync(imagemId);

            // Assert
            resultado.Should().BeTrue();
            _imagens.Should().NotContain(i => i.Id == imagemId); 
        }

    }
}
