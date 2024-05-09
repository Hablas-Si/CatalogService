using System.Collections.Generic;
using Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;



namespace CatalogService.Repository
{
    public class CatalogRepository : ICatalogRepository
    {

        private readonly IMongoCollection<Catalog> _catalog;

          public CatalogRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            // trækker connection string og database navn og collectionname fra program.cs aka fra terminalen ved export. Dette er en constructor injection.
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _catalog = database.GetCollection<Catalog>(mongoDBSettings.Value.CollectionName);

        }


        public IEnumerable<Catalog> getAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Catalog> getItem(int itemId)
        {
            return await _catalog.Find(c => c.ItemId == itemId).FirstOrDefaultAsync();
        }

        public Task createItemAsync(Catalog newItem)
        {
            throw new NotImplementedException();
        }
     
      
    }
}