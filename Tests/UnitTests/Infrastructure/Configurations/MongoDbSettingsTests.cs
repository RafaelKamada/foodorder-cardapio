using Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Tests.UnitTests.Infrastructure.Configurations
{
    public class MongoDbSettingsTests
    {

        //[Fact]
        //public void Should_Use_Default_Values_When_No_Configuration()
        //{
        //    // Arrange
        //    var configuration = new ConfigurationBuilder()
        //        .AddEnvironmentVariables()
        //        .Build();

        //    var serviceProvider = new ServiceCollection()
        //        .Configure<MongoDbSettings>(options =>
        //        {
        //            options.ConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION") ??
        //                configuration["MongoDbSettings:ConnectionString"];
        //            options.DatabaseName = configuration["MongoDbSettings:DatabaseName"] ?? "FoodOrder_Cardapio";
        //        })
        //        .BuildServiceProvider();

        //    var options = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;

        //    // Act
        //    var connectionString = options.ConnectionString;
        //    var databaseName = options.DatabaseName;

        //    // Assert
        //    Assert.Null(connectionString);
        //    Assert.Equal("FoodOrder_Cardapio", databaseName);
        //}

        [Fact]
        public void Should_Configure_MongoDbSettings_Correctly_From_Configuration()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string>
            {
                { "MongoDbSettings:ConnectionString", "mongodb://localhost:27017" },
                { "MongoDbSettings:DatabaseName", "TestDatabase" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var serviceProvider = new ServiceCollection()
                .Configure<MongoDbSettings>(options =>
                {
                    options.ConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION") ??
                        configuration["MongoDbSettings:ConnectionString"];
                    options.DatabaseName = configuration["MongoDbSettings:DatabaseName"];
                })
                .BuildServiceProvider();

            var options = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;

            // Act
            var connectionString = options.ConnectionString;
            var databaseName = options.DatabaseName;

            // Assert
            Assert.Equal("mongodb://localhost:27017", connectionString); 
            Assert.Equal("TestDatabase", databaseName);  
        }

        [Fact]
        public void Should_Use_Environment_Variables_When_Configuration_Is_Empty()
        {
            // Arrange
            Environment.SetEnvironmentVariable("MONGODB_CONNECTION", "mongodb://localhost:27017");

            var inMemorySettings = new Dictionary<string, string>
            {
                { "MongoDbSettings:DatabaseName", "EnvDatabase" }
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .AddEnvironmentVariables() 
                .Build();

            var serviceProvider = new ServiceCollection()
                .Configure<MongoDbSettings>(options =>
                {
                    options.ConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION") ??
                        configuration["MongoDbSettings:ConnectionString"];
                    options.DatabaseName = configuration["MongoDbSettings:DatabaseName"];
                })
                .BuildServiceProvider();

            var options = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;

            // Act
            var connectionString = options.ConnectionString;
            var databaseName = options.DatabaseName;

            // Assert
            Assert.Equal("mongodb://localhost:27017", connectionString); 
            Assert.Equal("EnvDatabase", databaseName); 
        }
    }
}
