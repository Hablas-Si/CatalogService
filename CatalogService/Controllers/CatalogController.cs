using Microsoft.AspNetCore.Mvc;
using CatalogService.Repository;
using Models;

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
            _logger.LogInformation("Fetching all items from catalog");
            var catalogList = await _service.getAll();
            return Ok(catalogList);
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> getSpecificItem(Guid Id)
        {
            _logger.LogInformation("Fetching item with ID: {Id}", Id);
            try
            {
                var item = await _service.getSpecificItem(Id);
                if (item != null)
                {
                    return Ok(item);
                }
                else
                {
                    _logger.LogWarning("Item with ID: {Id} not found", Id);
                    return NotFound("Item not found."); // Returnerer 404 NotFound
                }
            }
            catch (Exception ex)
            {
                // Log eventuelle fejl
                _logger.LogError(ex, "An error occurred while fetching item with ID: {Id}", Id);
                return NotFound(ex.Message); // Returnerer 404 NotFound
            }
        }

        [HttpPost]
        public async Task<IActionResult> createItem(Catalog newItem)
        {
            _logger.LogInformation("Creating new item in catalog");
            if (newItem == null)
            {
                _logger.LogWarning("Item cannot be null");
                return BadRequest("Item cannot be null");
            }

            await _service.CreateCatalog(newItem);
            return Ok(newItem);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateCatalogAndExtended(Guid Id, Catalog updatedCatalog)
        {
            _logger.LogInformation("Updating catalog and extended catalog with ID: {Id}", Id);
            try
            {
                await _service.UpdateCatalog(Id, updatedCatalog);

                return Ok("Catalog and ExtendedCatalog updated successfully.");
            }
            catch (Exception ex)
            {   _logger.LogError(ex, "An error occurred while updating catalog and extended catalog with ID: {Id}", Id);
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCatalog(Guid Id)
        {
            try
            {
                _logger.LogWarning("Deleting catalog with ID: {Id}", Id);
                // Slet kataloget
                await _service.DeleteCatalog(Id);

                // Slet den tilknyttede extended catalog

                return Ok("Catalog and associated ExtendedCatalog deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting catalog with ID: {Id}", Id);
                return NotFound(ex.Message);
            }
        }








    }
}