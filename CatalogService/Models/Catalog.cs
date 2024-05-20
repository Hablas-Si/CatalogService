using System;

namespace Models
{
    public class Catalog
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public bool? ProductAvailable { get; set; }
        public string? Seller { get; set; }

        public Catalog()
        {

        }
    }

}
