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
        private readonly IMongoCollection<Catalog.ExtendedCatalogInfo> _extendedcatalogCollection;

        public CatalogRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _catalogCollection = database.GetCollection<Catalog>(mongoDBSettings.Value.CatalogCollectionName);
            _extendedcatalogCollection = database.GetCollection<Catalog.ExtendedCatalogInfo>(mongoDBSettings.Value.ExtendedCatalogCollectionName);
        }


//Oprettelse
       public async Task CreateCatalog(Catalog newCatalog)
{
        await _catalogCollection.InsertOneAsync(newCatalog);
    // Opret ExtendedCatalog med reference til Catalog
        await CreateExtendedCatalog(newCatalog);

}

public async Task CreateExtendedCatalog(Catalog newCatalog){
    
      Catalog.ExtendedCatalogInfo newExtendedCatalog = new Catalog.ExtendedCatalogInfo
    {
        CatalogId = newCatalog.Id,
        Id = Guid.NewGuid(),
        SoldDate = DateTime.UtcNow,
        AuctionAdmin = "Default Admin",
        Seller = "Default Seller", // Du kan indsætte de ønskede værdier her
        Buyer = "Default Buyer"   // Du kan indsætte de ønskede værdier her
    };

    // Opret ExtendedCatalog
        await _extendedcatalogCollection.InsertOneAsync(newExtendedCatalog);
}





  public async Task<IEnumerable<Catalog>> getAll()
{
    // Hent alle Catalog-objekter
    var catalogs = await _catalogCollection.Find(_ => true).ToListAsync();

    // Opret en liste til at gemme Catalog-objekter med tilhørende ExtendedCatalog-objekter
    var catalogList = new List<Catalog>();

    // Gennemgå hvert Catalog-objekt og hent tilhørende ExtendedCatalog-objekt baseret på CatalogId
    foreach (var catalog in catalogs)
    {
        var extendedCatalog = await _extendedcatalogCollection.Find(ec => ec.CatalogId == catalog.Id).FirstOrDefaultAsync();
        catalog.ExtendedCatalog = extendedCatalog;

        // Tilføj catalog til den nye liste
        catalogList.Add(catalog);
    }

    return catalogList;
}




        public Task<Catalog> getItem(int itemId)
        {
            throw new NotImplementedException();
        }
    }
}
