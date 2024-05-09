using Microsoft.AspNetCore.Mvc;
using CatalogService.Repository;
using Models;
using CatalogService.Exceptions;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly ICatalogRepository _service;

        public CatalogController(ILogger<CatalogController> logger, ICatalogRepository service)
        {
            _logger = logger;
            this._service = service;
        }

        [HttpGet("all")]
        public async Task<IActionResult> getAll()
        {
            var catalogList = await _service.getAll();
            return Ok(catalogList);
        }


         [HttpGet("{itemId}")]
        public async Task<IActionResult> getSpecificItem(int itemId)
        {
            try
            {
                var item = await _service.getSpecificItem(itemId);
                if (item != null)
                {
                    return Ok(item);
                }
                else
                {
                    return NotFound("Item not found."); // Returnerer 404 NotFound
                }
            }
            catch (CatalogNotFoundException ex)
            {
                // Log eventuelle fejl
                _logger.LogError(ex, "An error occurred while fetching item with ID: {ItemId}", itemId);
                return NotFound(ex.Message); // Returnerer 404 NotFound
            }
        }

        [HttpPost]
        public async Task<IActionResult> createItem(Catalog newItem)
        {
            if (newItem == null)
            {
                return BadRequest("Item cannot be null");
            }

            await _service.CreateCatalog(newItem);
            return Ok(newItem);
        }
    }
}