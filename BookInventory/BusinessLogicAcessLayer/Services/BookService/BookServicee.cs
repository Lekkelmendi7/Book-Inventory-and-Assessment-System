using AutoMapper;
using BookInventory.DataAccess.Database;
using BookInventory.DataAccess.Entities;
using BookInventory.LogicAcessLayer.Models.BookModels;
using Microsoft.EntityFrameworkCore;

namespace BookInventory.LogicAcessLayer.Services.BookService
{
    public class BookServicee : IBookService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BookServicee> _logger;

        public BookServicee(DatabaseContext context, IMapper mapper, ILogger<BookServicee> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /*   public async Task<IEnumerable<BookGetModel>> GetAllBooks()
           {
               try
               {
                   _logger.LogInformation("Fetching all books from the database.");
                   var books = await _context.Books.Include(a => a.Author).ToListAsync();
                   var mappedBooks = _mapper.Map<IEnumerable<BookGetModel>>(books);
                   return mappedBooks;
               } catch(Exception ex)
               {
                   _logger.LogError(ex, "An error occurred while trying to fetch books.");
                   throw new Exception("An error occuredd while trying to fetch books.", ex);
               }
           }
         */

        public async Task<IEnumerable<BookGetModel>> GetAllBooks()
        {
            try
            {
                _logger.LogInformation("Fetching all books from the database.");
                var books = await _context.Books.Include(a => a.Author).ToListAsync();
                var mappedBooks = _mapper.Map<IEnumerable<BookGetModel>>(books);
                _logger.LogInformation("Exiting GetAllBooks method with {Count} books fetched.", mappedBooks.Count());
                return mappedBooks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to fetch books.");
                throw new Exception("An error occurred while trying to fetch books.", ex);
            }
        }
        public async Task<BookGetModel> GetBookById(int id)
        {
            _logger.LogInformation($"Fetching book with ID {id} from the database.");
            var book = await _context.Books.Include(a => a.Author).FirstOrDefaultAsync(x => x.Id == id);
            if(book == null)
            {
                _logger.LogWarning($"Book with ID {id} not found.");
                throw new KeyNotFoundException($"Book with ID {id} not found!");
            }
            var mappedBook = _mapper.Map<BookGetModel>(book);
            return mappedBook;
        }
        public async Task CreateBook(BookCreateModel bookModel)
        {
            try
            {
                _logger.LogInformation("Creating a new book in the database.");
                var bookEntity = new Book
                {
                    Title = bookModel.Title,
                    PublicationYear = bookModel.PublicationYear,
                    Genres = bookModel.Genres,
                    Language = bookModel.Language,
                    PageCount = bookModel.PageCount,
                    Price = bookModel.Price,
                    Stock = bookModel.Stock,
                    Edition = bookModel.Edition,
                    Format = bookModel.Format,
                    AuthorId = bookModel.AuthorId,
                };

                _context.Books.Add(bookEntity);
                await _context.SaveChangesAsync();
            } catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while trying to create a book.");
                throw new Exception("Error while trying to create a book", ex);
            }
        }


        public async Task UpdateBook(int id, BookUpdateModel bookModel)
        {
            _logger.LogInformation($"Updating book with ID {id}.");
            var newBook = await _context.Books.FirstOrDefaultAsync(_ => _.Id == id);

            if(newBook == null)
            {
                _logger.LogWarning($"Book with ID {id} not found.");
                throw new KeyNotFoundException($"Book with ID {id} not found!");
            }
            
            newBook.Title = bookModel.Title;    
            newBook.PublicationYear = bookModel.PublicationYear;
            newBook.Genres = bookModel.Genres;
            newBook.Language = bookModel.Language;
            newBook.PageCount = bookModel.PageCount;
            newBook.Price = bookModel.Price;
            newBook.Stock = bookModel.Stock;
            newBook.Edition = bookModel.Edition;
            newBook.Format = bookModel.Format;
            newBook.AuthorId = bookModel.AuthorId;  

            _context.Books.Update(newBook);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Book with ID {id} updated successfully.");

        }

        public async Task<bool> DeleteBook(int id)
        {
            _logger.LogInformation($"Deleting book with ID {id}.");
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                _logger.LogWarning($"Book with ID {id} not found.");
                throw new KeyNotFoundException($"Book with ID {id} not found!");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Book with ID {id} deleted successfully.");
            return true;
        }

    }
}
