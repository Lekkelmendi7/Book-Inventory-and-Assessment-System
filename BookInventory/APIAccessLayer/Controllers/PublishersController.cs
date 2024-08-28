using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Extensibility;
using BookInventory.BusinessLogicAcessLayer.Models.PulisherModels;
using BookInventory.BusinessLogicAcessLayer.Services.PublisherService;

namespace PublisherInventory.APIAccessLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherService _service;
        private readonly ILogger<PublishersController> _logger;

        public PublishersController(IPublisherService service, ILogger<PublishersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("getAllPublishers")]
        [Authorize(Policy = "Publisher_Read")]
        public async Task<IActionResult> GetAllPublishers()
        {
            try
            {
                _logger.LogInformation("API call to get all publishers started.");
                var publishers = await _service.GetPublishers();
                _logger.LogInformation("Successfully retrieved all publishers.");
                return Ok(publishers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all publishers.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpGet("getPublisherById/{id}")]
        [Authorize(Policy = "Publisher_Read")]
        public async Task<IActionResult> GetPublisherById(int id)
        {
            try
            {
                _logger.LogInformation($"API call to get publisher by ID {id} started.");
                var publisher = await _service.GetPublisher(id);
                if (publisher == null)
                {
                    _logger.LogWarning($"Publisher with ID {id} not found.");
                    return NotFound($"Publisher with ID {id} not found.");
                }
                _logger.LogInformation($"Successfully retrieved publisher with ID {id}.");
                return Ok(publisher);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving publisher with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpPost("addPublisher")]
        [Authorize(Policy = "Publisher_Create")]
        public async Task<IActionResult> AddPublisher(PublisherCreateModel model)
        {
            try
            {
                _logger.LogInformation("API call to add a new publisher started.");
                await _service.AddPublisher(model);
                _logger.LogInformation("Publisher added successfully.");
                return Ok("Publisher added successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new publisher.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpPut("updatePublisher/{id}")]
        [Authorize(Policy = "Publisher_Edit")]
        public async Task<IActionResult> UpdatePublisher(int id, PublisherUpdateModel model)
        {
            try
            {
                _logger.LogInformation($"API call to update publisher with ID {id} started.");
                await _service.UpdatePublisher(id, model);
                _logger.LogInformation($"Publisher with ID {id} updated successfully.");
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating publisher with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }


        [HttpDelete("deletePublisher/{id}")]
        [Authorize(Policy = "Publisher_Delete")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            try
            {
                _logger.LogInformation($"API call to delete publisher with ID {id} started.");
                var isDeleted = await _service.DeletePublisher(id);
                if (!isDeleted)
                {
                    _logger.LogWarning($"Publisher with ID {id} not found.");
                    return NotFound($"Publisher with ID {id} not found.");
                }
                _logger.LogInformation($"Publisher with ID {id} deleted successfully.");
                return Ok("Publisher deleted successfully!");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"Publisher with ID {id} not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting the publisher.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
    }
}
