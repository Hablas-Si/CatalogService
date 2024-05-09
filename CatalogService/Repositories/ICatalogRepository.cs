using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace CatalogService.Repository
{
    public interface ICatalogRepository
    {
        Task<IEnumerable<ExtendedCatalog>> getAll();
        Task<ExtendedCatalog> getItem(int itemId);
        Task CreateCatalogAndExtendedCatalogAsync(Catalog newItem);
    }
}