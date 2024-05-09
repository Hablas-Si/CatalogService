using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace CatalogService.Repository
{
    public interface ICatalogRepository
    {
        // Hent alle elementer
        public IEnumerable<Catalog> getAll();

        public Task<Catalog> getItem(int itemId);

        public Task createItemAsync(Catalog newItem);


    }
}