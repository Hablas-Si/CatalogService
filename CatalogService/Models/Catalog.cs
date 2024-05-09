using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Catalog
    {
        public Guid Id { get; set; }

        public int? ItemId { get; set; }

        public string? Name { get; set; }

        public decimal? Price { get; set; }

        
       
    }
}