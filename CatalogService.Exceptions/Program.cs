namespace CatalogService.Exceptions
{
    public class CatalogNotFoundException : Exception
    {
        public CatalogNotFoundException() : base("Catalog Not Found")
        {
        }
        public CatalogNotFoundException(string message) : base(message) 
        {
        }
    }
}