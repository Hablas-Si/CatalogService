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

        [HttpGet]
        public IActionResult getAll()
        {
            var list = _service.getAll();
            return Ok(list);
        }

        [HttpGet("{itemId}")]
        public IActionResult getItem(int itemId)
        {
            try
            {
                var item = _service.getItem(itemId);
                return Ok(item);
            }
            catch (CatalogNotFoundException ex)
            {
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

            await _service.createItemAsync(newItem);
            return Ok(newItem);
        }
    }
}