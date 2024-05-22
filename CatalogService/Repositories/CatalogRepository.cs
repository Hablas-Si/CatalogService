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
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionAuctionDB);
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


        public async Task<Catalog> getSpecificItem(Guid Id)
        {
            try
            {
                // Søg efter vareelement (item) baseret på itemId
                var catalogItem = await _catalogCollection.Find(c => c.Id == Id).FirstOrDefaultAsync();
                if (catalogItem == null)
                {
                    throw new Exception();
                }

                return catalogItem;
            }
            catch (Exception ex)
            {
                throw new Exception(); // Kast undtagelsen videre, hvis kataloget ikke blev fundet
            }
        }


        public async Task DeleteCatalog(Guid Id)
        {
            try
            {
                // Find det katalog, der skal slettes baseret på ItemId
                var filter = Builders<Catalog>.Filter.Eq(c => c.Id, Id);

                // Slet kataloget
                await _catalogCollection.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                // Håndter eventuelle undtagelser her
                throw new Exception("An error occurred while deleting the catalog.", ex);
            }
        }
        public async Task UpdateCatalog(Guid Id, Catalog updatedItem)
        {
            try
            {
                // Opret filter til at finde det katalogelement, der skal opdateres
                var filter = Builders<Catalog>.Filter.Eq(c => c.Id, Id);

                // Opret opdatering med de opdaterede oplysninger, inklusive den udvidede kataloginfo
                var update = Builders<Catalog>.Update
                    .Set(c => c.Title, updatedItem.Title)
                    .Set(c => c.Description, updatedItem.Description)
                    .Set(c => c.Price, updatedItem.Price)
                    .Set(c => c.ProductAvailable, updatedItem.ProductAvailable)
                    .Set(c => c.Seller, updatedItem.Seller);
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
