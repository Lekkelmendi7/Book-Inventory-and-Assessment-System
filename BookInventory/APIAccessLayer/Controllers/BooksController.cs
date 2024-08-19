using BookInventory.LogicAcessLayer.Models.BookModels;
using BookInventory.LogicAcessLayer.Services.BookService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookInventory.APIAccessLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;

        public BooksController(IBookService service)
        {
            _service = service;
        }


        [HttpGet("getAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            var autors = await _service.GetAllBooks();
            return Ok(autors);  
        }

        [HttpGet("getSingleBook/{id}")]
        public async Task<IActionResult> GetSingleBook(int id)
        {
            var book = await _service.GetBookById(id);
            if(book == null)
            {
                return NotFound("Book you're looking, not found!");
            }
            return Ok(book);    
        }

        [HttpPost("addBook")]
        public async Task<IActionResult> AddBook(BookCreateModel model)
        {
            await _service.CreateBook(model);
            return Ok("Book added succesfully");   
        }

        [HttpPut("updateBook/{id}")]
        public async Task<IActionResult> UpdateBook(int id, BookUpdateModel model)
        {
            await _service.UpdateBook(id, model);
            return Ok("Book updated!");    
        }

        [HttpDelete("deleteBook/{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var isDeleted = await _service.DeleteBook(id);

            if (!isDeleted)
            {
                return NotFound("Book not found!");
            }

            return Ok("Book deleted!");
        }

    }
}
