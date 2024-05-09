using System;

namespace Models
{
    public class Catalog
    {
        public Guid Id { get; set; }

        public int? ItemId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }
    }
}
