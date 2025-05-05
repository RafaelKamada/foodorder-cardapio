using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.UnitTests.Infrastructure.Repositories
{
    public class RepositoryTestBase : IDisposable
    {
        protected IMongoClient Client { get; }
        protected IMongoDatabase Database { get; }
        private bool disposedValue;

        public RepositoryTestBase()
        {
            // Setup do MongoDB InMemory
            Client = new MongoClient("mongodb://localhost:27017");
            Database = Client.GetDatabase("test");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Limpar dados após os testes
                    Database.DropCollection("produtos");
                    Database.DropCollection("categorias");
                    Database.DropCollection("imagens");
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
