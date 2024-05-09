using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class MongoDBSettings
    {
        public string ConnectionURI { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CatalogCollectionName { get; set; } = null!;
        public string ExtendedCatalogCollectionName { get; set; } = null!;
    }

}