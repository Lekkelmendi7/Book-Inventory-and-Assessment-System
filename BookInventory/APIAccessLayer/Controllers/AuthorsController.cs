﻿using BookInventory.LogicAcessLayer.Models.AuthorModels;
using BookInventory.LogicAcessLayer.Services.AuthorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookInventory.APIAccessLayer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _service;
        private readonly ILogger<AuthorsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorsController"/> class.
        /// </summary>
        /// <param name="service">The author service.</param>

        public AuthorsController(IAuthorService service, ILogger<AuthorsController> logger)
        {
            _service = service;
            _logger = logger;
        }
        /// <summary>
        /// Gets all authors.
        /// </summary>
        /// <returns>A list of authors.</returns>
        /// 
        [HttpGet("getAllAuthors")]
        [Authorize(Policy = "Author_Read")]
        public async Task<IActionResult> GetAllAuthors([FromQuery] int page, [FromQuery] int size, [FromQuery] string? sortBy, [FromQuery] string? sortOrder = "asc")
        {
            try
            {
                _logger.LogInformation("Displaying all authors!");
                var authors = await _service.GetAllAuthors(page, size, sortBy, sortOrder);
                _logger.LogInformation("All authors displayed!");
                return Ok(authors);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while trying to fetch all authors!");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets a single author by identifier.
        /// </summary>
        /// <param name="id">The identifier of the author.</param>
        /// <returns>The author with the specified identifier.</returns>
        [HttpGet("findById/{id}")]
        [Authorize(Policy = "Author_Read")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            try
            {
                _logger.LogInformation($"Displaying author with id {id}!");
                var author = await _service.GetAuthorById(id);
                return Ok(author);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Author not found!");
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                _logger.LogError("Error while trying to fetch the author.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Adds a new author.
        /// </summary>
        /// <param name="model">The author model.</param>
        /// <returns>The created author.</returns>
        [HttpPost("addAuthor")]
        [Authorize(Policy = "Author_Create")]
        public async Task<IActionResult> AddAuthor(AuthorCreateModel model)
        {
            try
            {
                _logger.LogInformation("Adding the author");
                await _service.CreateAuthor(model);
                return Ok("Author added successfully!");
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error while trying to create an author!");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Updates an existing author.
        /// </summary>
        /// <param name="id">The identifier of the author to update.</param>
        /// <param name="author">The author model with updated data.</param>
        /// <returns>A success message if the author was updated successfully.</returns>
        [HttpPut("updateAuthor/{id}")]
        [Authorize(Policy = "Author_Update")]
        public async Task<IActionResult> UpdateAuthor(int id, AuthorUpdateModel author)
        {
            try
            {
                _logger.LogInformation("Updating the author!");
                await _service.UpdateAuthor(id, author);
                return Ok("Author updated successfully!");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Not found!");
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                _logger.LogError("Error while trying yo update the author!");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Deletes a single author by identifier.
        /// </summary>
        /// <param name="id">The identifier of the author to delete.</param>
        /// <returns>A success message if the author was deleted successfully.</returns>

        [HttpDelete("deleteAuthor/{id}")]
        [Authorize(Policy = "Author_Delete")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                _logger.LogInformation($"API call to delete author with ID {id} started.");
                var isDeleted = await _service.DeleteAuthor(id);
                _logger.LogInformation($"Author with ID {id} deleted successfully.");
                return Ok("Author deleted successfully!");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"Author with ID {id} not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting the author.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("getAuthorsByNationality")]
        [Authorize(Policy = "Author_Read")]
        public async Task<IActionResult> GetAuthorsByNationality([FromQuery] string nationality)
        {
            try
            {
                var filteredAuthors = await _service.GetAuthorsByNationality(nationality);

                if (!filteredAuthors.Any())
                {
                    return NotFound($"No authors found with nationality: {nationality}");
                }

                return Ok(filteredAuthors);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching authors by nationality.");
                return StatusCode(500, "An error occurred while fetching authors by nationality.");
            }
        }

    }
}