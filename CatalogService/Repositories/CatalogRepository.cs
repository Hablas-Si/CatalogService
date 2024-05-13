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

        public CatalogRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _catalogCollection = database.GetCollection<Catalog>(mongoDBSettings.Value.CatalogCollectionName);
        }


        //Oprettelse
        public async Task CreateCatalog(Catalog newCatalog)
        {
            await _catalogCollection.InsertOneAsync(newCatalog);
            // Opret ExtendedCatalog med reference til Catalog

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
                // Tilføj catalog til den nye liste
                catalogList.Add(catalog);
            }

            return catalogList;
        }


        public async Task<Catalog> getSpecificItem(int itemId)
        {
            try
            {
                // Søg efter vareelement (item) baseret på itemId
                var catalogItem = await _catalogCollection.Find(c => c.ItemId == itemId).FirstOrDefaultAsync();
                if (catalogItem == null)
                {
                    throw new Exception();
                }

                return catalogItem;
            }
            catch (Exception ex)
            {
                throw; // Kast undtagelsen videre, hvis kataloget ikke blev fundet
            }
        }


        public async Task DeleteCatalog(int itemId)
        {
            try
            {
                // Find det katalog, der skal slettes baseret på ItemId
                var filter = Builders<Catalog>.Filter.Eq(c => c.ItemId, itemId);

                // Slet kataloget
                await _catalogCollection.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                // Håndter eventuelle undtagelser her
                throw new Exception("An error occurred while deleting the catalog.", ex);
            }
        }
        public async Task UpdateCatalog(int itemId, Catalog updatedItem)
        {
            try
            {
                // Opret filter til at finde det katalogelement, der skal opdateres
                var filter = Builders<Catalog>.Filter.Eq(c => c.ItemId, itemId);

                // Opret opdatering med de opdaterede oplysninger, inklusive den udvidede kataloginfo
                var update = Builders<Catalog>.Update
                    .Set(c => c.Name, updatedItem.Name)
                    .Set(c => c.Description, updatedItem.Description)
                    .Set(c => c.Price, updatedItem.Price)
                    .Set(c => c.ExtendedCatalog.SoldDate, updatedItem.ExtendedCatalog.SoldDate)
                    .Set(c => c.ExtendedCatalog.AuctionAdmin, updatedItem.ExtendedCatalog.AuctionAdmin)
                    .Set(c => c.ExtendedCatalog.Seller, updatedItem.ExtendedCatalog.Seller)
                    .Set(c => c.ExtendedCatalog.Buyer, updatedItem.ExtendedCatalog.Buyer);

                // Udfør opdateringen i MongoDB
                await _catalogCollection.UpdateOneAsync(filter, update);
            }
            catch (Exception ex)
            {
                // Håndter eventuelle undtagelser her
                throw new Exception("An error occurred while updating the catalog item.", ex);
            }
        }

    }
}
