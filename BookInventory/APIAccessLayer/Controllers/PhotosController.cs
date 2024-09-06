using BookInventory.BusinessLogicAcessLayer.Services.PhotoService;
using BookInventory.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookInventory.APIAccessLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotosController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        // GET: api/photo
        [HttpGet]
        public async Task<IActionResult> GetAllPhotos()
        {
            var photos = await _photoService.GetAllAsync();
            return Ok(photos); // Return 200 OK with the list of photos
        }

        // GET: api/photo/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhotoById(string id)
        {
            var photo = await _photoService.GetByIdAsync(id);

            if (photo == null)
            {
                return NotFound(); // Return 404 if no photo is found
            }

            return Ok(photo); // Return 200 OK with the photo
        }

        // POST: api/photo
        [HttpPost]
        public async Task<IActionResult> AddPhoto([FromBody] BookPhoto bookPhoto)
        {
            if (bookPhoto == null)
            {
                return BadRequest("Invalid photo data."); // Return 400 if the input is null
            }

            try
            {
                var createdPhoto = await _photoService.AddAsync(bookPhoto);
                return CreatedAtAction(nameof(GetPhotoById), new { id = createdPhoto.Id }, createdPhoto); // Return 201 Created with the new photo
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Return 500 on exception
            }
        }

        // PUT: api/photo/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePhoto(string id, [FromBody] BookPhoto bookPhoto)
        {
            if (bookPhoto == null || bookPhoto.Id != id)
            {
                return BadRequest("Photo data is invalid or ID mismatch."); // Return 400 if the input is invalid
            }

            try
            {
                var updateResult = await _photoService.UpdateAsync(bookPhoto);

                if (!updateResult)
                {
                    return NotFound(); // Return 404 if the photo was not found for updating
                }

                return Ok(); // Return 200 OK if the update is successful
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Return 500 on exception
            }
        }

        // DELETE: api/photo/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid photo ID."); // Return 400 if the ID is null or empty
            }

            try
            {
                var deleteResult = await _photoService.DeleteByIdAsync(id);

                if (!deleteResult)
                {
                    return NotFound(); // Return 404 if the photo was not found for deletion
                }

                return NoContent(); // Return 204 No Content if the deletion is successful
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Return 500 on exception
            }
        }
    }
}
