using MongoDB.Driver;

namespace Infrastructure.Configurations
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
