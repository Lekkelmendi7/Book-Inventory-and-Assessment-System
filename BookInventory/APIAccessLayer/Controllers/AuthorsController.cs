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

        public AuthorsController(IAuthorService service) 
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
           var authors = await _service.GetAllAuthors();
            return Ok(authors); 
        }

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

        [HttpPost]
        public async Task<IActionResult> AddAuthor(AuthorCreateModel model)
        {
            await _service.CreateAuthor(model); 
            return Ok("Author added successfully!");
        }

        [HttpPut("updateAuthor/{id}")]
        public async Task<IActionResult> UpdateAuthor(AuthorUpdateModel author,int id)
        {
            await _service.UpdateAuthor(id, author);
            return Ok(author);  
        }

        [HttpDelete("deleteAuthor/{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = _service.DeleteAuthor(id); 
            if(author == null)
            {
                return BadRequest("Author not found!");
            }
            return Ok("Author deleted!");  
        }

    }
}
