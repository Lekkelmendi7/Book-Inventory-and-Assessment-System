﻿using BookInventory.BusinessLogicAcessLayer.Services.PhotoService;
using BookInventory.DataAccess.Entities;
using BookInventory.LogicAcessLayer.Models.AuthorModels;
using BookInventory.LogicAcessLayer.Models.BookModels;
using BookInventory.LogicAcessLayer.Services.BookService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookInventory.APIAccessLayer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;
        private readonly ILogger<BooksController> _logger;
        private readonly IPhotoService _photoService;

        public BooksController(IBookService service, ILogger<BooksController> logger, IPhotoService photoService)
        {
            _service = service;
            _logger = logger;
            _photoService = photoService;
        }

        [HttpGet("getAllBooks")]
        [Authorize(Policy = "Book_Read")]
        public async Task<IActionResult> GetAllBooks([FromQuery] int page, [FromQuery] int size, [FromQuery] string? sortBy, [FromQuery] string? sortOrder = "asc")
        {
            try
            {
                _logger.LogInformation("API call to get all books started.");
                var books = await _service.GetAllBooks(page, size, sortOrder, sortBy);
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
                if (book == null)
                {
                    return NotFound($"Book with ID {id} not found.");
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost("addBook")]
        [Authorize(Policy = "Book_Create")]
        public async Task<IActionResult> AddBook([FromBody] BookCreateModel model)
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
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookUpdateModel model)
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
                if (!isDeleted)
                {
                    return NotFound($"Book with ID {id} not found.");
                }
                return Ok("Book deleted successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting a book.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("getBooksByPublicationYear")]
        //[Authorize(Policy = "Book_Read")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBooksByPublicationYear([FromQuery] int? publicationYear)
        {
            try
            {
                _logger.LogInformation($"API call to get books by publication year: {publicationYear}.");
                var books = await _service.GetBooksByPublicationYear(publicationYear);
                if (books == null || !books.Any())
                {
                    return Ok(books);
                }
                return Ok(books);
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
                _logger.LogInformation($"API call to get books by language: {language}.");
                var books = await _service.GetBooksByLanguage(language);
                if (books == null || !books.Any())
                {
                    return NotFound($"No books found for language {language}.");
                }
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching books by language.");
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

                _logger.LogInformation($"API call to get books by genres: {string.Join(", ", genres)}.");
                var books = await _service.SelectBooksByGenres(genres);
                if (books == null || !books.Any())
                {
                    return NotFound($"No books found for genres: {string.Join(", ", genres)}.");
                }
                return Ok(books);
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
                _logger.LogInformation($"API call to search books by name: {searchByName}.");
                var books = await _service.SearchBook(searchByName);
                if (books == null || !books.Any())
                {
                    return NotFound($"No books found for name: {searchByName}.");
                }
                return Ok(books);
            }
            catch (Exception ex)
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
                _logger.LogInformation($"API call to get books by author name: {authorName}.");
                var books = await _service.GetBooksByAuthorName(authorName);
                if (books == null || !books.Any())
                {
                    return NotFound($"No books found for author: {authorName}.");
                }
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching books by author name.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        #region Photo Management

        [HttpGet("getBookPhoto/{bookId}")]
        [Authorize(Policy = "Book_Read")]
        public async Task<IActionResult> GetBookPhoto(int bookId)
        {
            try
            {
                _logger.LogInformation($"API call to get photo for book with ID {bookId}.");
                var photo = await _photoService.GetBookPhotoById(bookId);
                if (photo == null)
                {
                    return NotFound($"Photo not found for book with ID {bookId}.");
                }
                return Ok(photo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching the book photo.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost("addBookPhoto")]
        [Authorize(Policy = "Book_Create")]
        public async Task<IActionResult> AddBookPhoto([FromQuery] int bookId, [FromQuery] string urlPhoto, [FromQuery] string photoName)
        {
            try
            {
                _logger.LogInformation($"API call to add photo {photoName} for book with ID {bookId}.");
                await _photoService.AddBookPhoto(bookId, urlPhoto, photoName);
                return Ok("Photo added successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding a book photo.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut("updateBookPhoto/{bookId}")]
        [Authorize(Policy = "Book_Edit")]
        public async Task<IActionResult> UpdateBookPhoto(int bookId, [FromQuery]string urlPhoto, [FromQuery] string photoName)
        {
            try
            {
                _logger.LogInformation($"API call to update photo for book with ID {bookId} to {photoName}.");
                await _photoService.UpdateBookPhoto(bookId, urlPhoto, photoName);
                return Ok("Photo updated successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating the book photo.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

      


        #endregion
    }
}
