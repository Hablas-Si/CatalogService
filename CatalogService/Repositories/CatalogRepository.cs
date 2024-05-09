using System;
using System.Threading.Tasks;
using Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CatalogService.Repository
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly IMongoCollection<Catalog> _catalogCollection;
        private readonly IMongoCollection<ExtendedCatalog> _extendedCatalogCollection;

        public CatalogRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _catalogCollection = database.GetCollection<Catalog>(mongoDBSettings.Value.CatalogCollectionName);
            _extendedCatalogCollection = database.GetCollection<ExtendedCatalog>(mongoDBSettings.Value.ExtendedCatalogCollectionName);
        }

        public async Task CreateCatalogAndExtendedCatalogAsync(Catalog newCatalog)
        {
            // Opret Catalog
            await _catalogCollection.InsertOneAsync(newCatalog);

            // Opret ExtendedCatalog med reference til Catalog
            ExtendedCatalog newExtendedCatalog = new ExtendedCatalog
            {
                Id = Guid.NewGuid(),
                CatalogId = newCatalog.Id,
                Date = DateTime.UtcNow,
                Seller = "Default Seller", // Du kan indsætte de ønskede værdier her
                Buyer = "Default Buyer"    // Du kan indsætte de ønskede værdier her
            };

            await _extendedCatalogCollection.InsertOneAsync(newExtendedCatalog);
        }


        public Task<IEnumerable<ExtendedCatalog>> getAll()
        {
            throw new NotImplementedException();
        }

        public Task<ExtendedCatalog> getItem(int itemId)
        {
            throw new NotImplementedException();
        }
    }
}
