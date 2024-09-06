using BookInventory.DataAccess.Entities;
using BookInventory.LogicAcessLayer.Models.AuthorModels;
using BookInventory.LogicAcessLayer.Models.BookModels;
using BookInventory.LogicAcessLayer.Services.BookService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookInventory.APIAccessLayer.Controllers
{
    [Authorize]
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
        [Authorize(Policy = "Book_Read")]
        public async Task<IActionResult> GetAllBooks([FromQuery] int page, [FromQuery] int size)
        {
            try
            {
                _logger.LogInformation("API call to get all books started.");
                var books = await _service.GetAllBooks(page, size);
                _logger.LogInformation("API call to get all books completed successfully!");
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all books from the API.");
                return StatusCode(500, "An unexpected error occurred while fetching books.");
            }
        }

        [HttpGet("getSingleBook/{id}")]
        [Authorize(Policy = "Book_Read")]
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
        [Authorize(Policy = "Book_Create")]
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
        [Authorize(Policy = "Book_Edit")]
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
        [Authorize(Policy = "Book_Delete")]
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

        [HttpGet("getBooksByPublicationYear/{publicationYear}")]
        [Authorize(Policy = "Book_Read")]
        public async Task<IActionResult> GetBooksByPublicationYear(int publicationYear)
        {
            try
            {
                _logger.LogInformation($"API call to get books by publication year: {publicationYear}.");
                var books = await _service.GetBooksByPublicationYear(publicationYear);
                return Ok(books);
            }
            catch(KeyNotFoundException ex)
            {
                _logger.LogWarning($"Book/s with that publication year, not found!");
                return NotFound(ex.Message );   
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching books by publication year.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("getBooksByLanguage/{language}")]
        [Authorize(Policy = "Book_Read")]
        public async Task<IActionResult> GetBooksByLanguage(string language)
        {
            try
            {
                _logger.LogInformation($"API call to get books by language!");
                var books = await _service.GetBooksByLanguage(language);
                return Ok(books);
            }catch(KeyNotFoundException ex)
            {
                _logger.LogWarning($"Book/s with that language, not found!");
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching books by publication year.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("getBooksByGenres")]
        [Authorize(Policy = "Book_Read")]
        public async Task<IActionResult> GetBooksByGenres([FromQuery] string[] genres)
        {
            try
            {
                if (genres == null || genres.Length == 0)
                {
                    _logger.LogWarning("No genres provided in the request.");
                    return BadRequest("Genres are required.");
                }

                _logger.LogInformation($"API call to get books by genres: {string.Join(", ", genres)}");
                var books = await _service.SelectBooksByGenres(genres);

                if (books == null || !books.Any())
                {
                    _logger.LogWarning($"No books found for the provided genres: {string.Join(", ", genres)}");
                    return NotFound("No books found for the provided genres.");
                }

                return Ok(books);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"Books with selected genres not found: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching books by genres.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("searchBook")]
        [Authorize(Policy = "Book_Read")]
        public async Task<IActionResult> SearchBookByName([FromQuery] string searchByName, [FromQuery] string sort)
        {
            try
            {
                _logger.LogInformation($"API call to search books by name: {searchByName}");
                var books = await _service.SearchBook(searchByName);
                if(books == null)
                {
                    throw new KeyNotFoundException("Book that you were looking for was not found!");
                }

                return Ok(books);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while searching for books by name.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [HttpGet("getBooksByAuthorName")]
        [Authorize(Policy = "Book_Read")]
        public async Task<IActionResult> GetBooksByAuthorName([FromQuery] string authorName)
        {
            try
            {
                _logger.LogInformation($"API call to get books by author name: {authorName}");
                var books = await _service.GetBooksByAuthorName(authorName);

                if (books == null || !books.Any())
                {
                    _logger.LogWarning($"No books found for author: {authorName}");
                    return NotFound($"No books found for author: {authorName}");
                }

                return Ok(books);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Invalid author name provided: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching books by author name.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


    }
}
