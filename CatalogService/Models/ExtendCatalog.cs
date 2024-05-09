using System;

namespace Models
{
    public class ExtendedCatalog
    {
        public Guid Id { get; set; }
        public Guid CatalogId { get; set; } // Reference til Catalog
        public DateTime? SoldDate { get; set; }
        public string? AuctionAdmin { get; set; }
        public string? Seller { get; set; }
        public string? Buyer { get; set; }
    }
}
