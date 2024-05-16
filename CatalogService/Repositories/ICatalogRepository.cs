using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace CatalogService.Repository
{
    public interface ICatalogRepository
    {
        Task<IEnumerable<Catalog>> getAll();
        Task<Catalog> getSpecificItem(Guid itemId);
        Task CreateCatalog(Catalog newItem);

        Task UpdateCatalog(Guid itemId, Catalog updatedItem);
        Task DeleteCatalog(Guid itemId);


    }
}