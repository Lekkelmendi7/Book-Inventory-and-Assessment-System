using BookInventory.LogicAcessLayer.Models.AuthorModels;
using BookInventory.LogicAcessLayer.Services.AuthorService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookInventory.APIAccessLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorsController"/> class.
        /// </summary>
        /// <param name="service">The author service.</param>

        public AuthorsController(IAuthorService service) 
        {
            _service = service;
        }
        /// <summary>
        /// Gets all authors.
        /// </summary>
        /// <returns>A list of authors.</returns>
        /// 
        [HttpGet("getAllAuthors")]
        public async Task<IActionResult> GetAllAuthors()
        {
           var authors = await _service.GetAllAuthors();
            return Ok(authors); 
        }

        /// <summary>
        /// Gets a single author by identifier.
        /// </summary>
        /// <param name="id">The identifier of the author.</param>
        /// <returns>The author with the specified identifier.</returns>
        [HttpGet("findById/{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = await _service.GetAuthorById(id);
            if(author == null)
            {
                return BadRequest("Author not found!");
            }
            return Ok(author);  
        }

        /// <summary>
        /// Adds a new author.
        /// </summary>
        /// <param name="model">The author model.</param>
        /// <returns>The created author.</returns>
        [HttpPost("addAuthor")]
        public async Task<IActionResult> AddAuthor(AuthorCreateModel model)
        {
            await _service.CreateAuthor(model); 
            return Ok("Author added successfully!");
        }

        /// <summary>
        /// Updates an existing author.
        /// </summary>
        /// <param name="id">The identifier of the author to update.</param>
        /// <param name="author">The author model with updated data.</param>
        /// <returns>A success message if the author was updated successfully.</returns>
        [HttpPut("updateAuthor/{id}")]
        public async Task<IActionResult> UpdateAuthor(AuthorUpdateModel author,int id)
        {
            await _service.UpdateAuthor(id, author);
            return Ok(author);  
        }

        /// <summary>
        /// Deletes a single author by identifier.
        /// </summary>
        /// <param name="id">The identifier of the author to delete.</param>
        /// <returns>A success message if the author was deleted successfully.</returns>

        [HttpDelete("deleteAuthor/{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var isDeleted = await _service.DeleteAuthor(id);

            if (!isDeleted)
            {
                return NotFound("Author not found!");
            }

            return Ok("Author deleted!");
        }
    }
}
