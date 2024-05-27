using Microsoft.AspNetCore.Mvc;
using CatalogService.Repositories;
using Models;
using Microsoft.AspNetCore.Authorization;


namespace CatalogService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly ICatalogRepository _service;
        private readonly IVaultRepository _vaultService;


        public CatalogController(ILogger<CatalogController> logger, ICatalogRepository service, IVaultRepository vaultservice)
        {
            _logger = logger;
            _service = service;
            _vaultService = vaultservice;
        }

        [HttpGet("all"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> getAll()
        {
            _logger.LogInformation("Fetching all items from catalog");
            var catalogList = await _service.getAll();
            return Ok(catalogList);
        }


        [HttpGet("{Id}"), Authorize(Roles = "Admin")]
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

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> createItem(Catalog newItem)
        {
            //Checks if item == null
            if (newItem == null)
            {
                _logger.LogWarning("Item cannot be null");
                return BadRequest("Item cannot be null");
            }
            // Checks for invalid inputs
            if (string.IsNullOrWhiteSpace(newItem.Title) || newItem.Price < 0)
            {
                _logger.LogWarning("Invalid item data");
                return BadRequest("Invalid item data");
            }

            await _service.CreateCatalog(newItem);
            return Ok(newItem);
        }

        [HttpPut("{Id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCatalogAndExtended(Guid Id, Catalog updatedCatalog)
        {
            _logger.LogInformation("Updating catalog and extended catalog with ID: {Id}", Id);
            try
            {
                await _service.UpdateCatalog(Id, updatedCatalog);

                return Ok("Catalog and ExtendedCatalog updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating catalog and extended catalog with ID: {Id}", Id);
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{Id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCatalog(Guid Id)
        {
            try
            {
                _logger.LogWarning("Deleting catalog with ID: {Id}", Id);

                // Check if the item exists
                var existingItem = await _service.getSpecificItem(Id);


                if (existingItem == null)
                {
                    _logger.LogError("Catalog not found with ID: {Id}", Id);
                    return NotFound("Catalog not found");
                }

                // Slet kataloget
                await _service.DeleteCatalog(Id);

                return Ok("Catalog and associated ExtendedCatalog deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting catalog with ID: {Id}", Id);
                return NotFound(ex);
            }
        }


        //TEST ENVIRONMENT
        // OBS: TIlføj en Authorize attribute til metoderne nedenunder Kig ovenfor i jwt token creation.
        [HttpGet("authorized"), Authorize(Roles = "Admin")]
        public IActionResult Authorized()
        {

            // Hvis brugeren har en gyldig JWT-token og rollen "Admin", vil denne metode blive udført
            return Ok("You are authorized to access this resource.");
        }





    }
}
