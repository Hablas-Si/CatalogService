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
        Task<Catalog> getSpecificItem(int itemId);
        Task CreateCatalog(Catalog newItem);
        Task CreateExtendedCatalog(Catalog newCatalog);


    }
}