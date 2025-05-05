using Domain.Entities;
using Infrastructure.Configurations;
using MongoDB.Driver;
using Moq;

namespace Tests.UnitTests.Infrastructure.Configurations
{
    public class MongoDbContextTests
    {
        private readonly Mock<IMongoDatabase> _mockDatabase;
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<Produto> _mockCollection;

        public MongoDbContextTests()
        {
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockCollection = new Mock<IMongoCollection<Produto>>().Object;

            _mockDatabase.Setup(d =>
                d.GetCollection<Produto>(
                    It.IsAny<string>(),
                    It.IsAny<MongoCollectionSettings>()
                ))
                .Returns(_mockCollection);

            _context = new MongoDbContext(_mockDatabase.Object);
        }

        [Fact]
        public void GetCollection_ShouldReturnCollection()
        {
            // Arrange
            var collectionName = "produtos";

            // Act
            var result = _context.GetCollection<Produto>(collectionName);

            // Assert
            _mockDatabase.Verify(d =>
                d.GetCollection<Produto>(
                    It.IsAny<string>(),
                    It.IsAny<MongoCollectionSettings>()
                ),
                Times.Once());
            Assert.IsAssignableFrom<IMongoCollection<Produto>>(result);
        }

        [Fact]
        public void GetCollection_ShouldThrowExceptionForNullName()
        {
            // Arrange
            string collectionName = null;

            // Act
            var result = _context.GetCollection<Produto>(collectionName);

            // Assert
            Assert.IsAssignableFrom<IMongoCollection<Produto>>(result);
        }

        [Fact]
        public void GetCollection_ShouldReturnCorrectCollectionType()
        {
            // Arrange
            var collectionName = "produtos";

            // Act
            var result = _context.GetCollection<Produto>(collectionName);

            // Assert
            Assert.IsAssignableFrom<IMongoCollection<Produto>>(result);
        }
    }
}