using BookInventory.DataAccess.Entities;
using BookInventory.LogicAcessLayer.Models.BookModels;
using BookInventory.LogicAcessLayer.Services.BookService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookInventory.APIAccessLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService service, ILogger<BooksController> logger)
        {
            _service = service;
            _logger = logger;
        }


        /* [HttpGet("getAllBooks")]
         public async Task<IActionResult> GetAllBooks()
         {
             try
             {
                 _logger.LogInformation("API call to get all books.");
                 var books = await _service.GetAllBooks();
                 return Ok(books);
             }
             catch (Exception ex)
             {
                 _logger.LogError(ex, "Error occurred while fetching all books.");
                 return BadRequest(ex.Message);
             }
         }*/
        [HttpGet("getAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                _logger.LogInformation("API call to get all books started.");
                var books = await _service.GetAllBooks();
                _logger.LogInformation("API call to get all books completed successfully with {Count} books returned.", books.Count());
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all books from the API.");
                return StatusCode(500, "An unexpected error occurred while fetching books.");
            }
        }

        [HttpGet("getSingleBook/{id}")]
        public async Task<IActionResult> GetSingleBook(int id)
        {
            try
            {
                _logger.LogInformation($"API call to get book with ID {id}.");
                var book = await _service.GetBookById(id);
                return Ok(book);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"Book with ID {id} not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost("addBook")]
        public async Task<IActionResult> AddBook(BookCreateModel model)
        {
            try
            {
                _logger.LogInformation("API call to add a new book.");
                await _service.CreateBook(model);
                return Ok("Book added successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding a book.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
        [HttpPut("updateBook/{id}")]
        public async Task<IActionResult> UpdateBook(int id, BookUpdateModel model)
        {
            try
            {
                _logger.LogInformation($"API call to update book with ID {id}.");
                await _service.UpdateBook(id, model);
                return Ok("Book updated successfully!");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"Book with ID {id} not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating a book.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete("deleteBook/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                _logger.LogInformation($"API call to delete book with ID {id}.");
                var isDeleted = await _service.DeleteBook(id);
                return Ok("Book deleted successfully!");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"Book with ID {id} not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting a book.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

    }
}
