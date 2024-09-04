using AutoMapper;
using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.BusinessLogicAcessLayer.Models;
using BookInventory.BusinessLogicAcessLayer.Models.BookModels;
using BookInventory.DataAccess.Database;
using BookInventory.DataAccess.Entities;
using BookInventory.LogicAcessLayer.Models.BookModels;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public async Task<PaginatedResult<BookGetModel>> GetAllBooks(int page, int size)
        {
            if (size > PaginationModel.MaxPageSize)
            {
                size = PaginationModel.MaxPageSize; // Ensure page size does not exceed max limit
            }

            try
            {
                _logger.LogInformation("Fetching books from the database with pagination.");

                // Define the query
                var queryable = _context.Books.Include(b => b.Author).AsQueryable();

                // Apply pagination
                var paginatedBooks = queryable.Paginate(page, size);

                // Map the results
                var mappedBooks = _mapper.Map<IEnumerable<BookGetModel>>(paginatedBooks.Items);

                _logger.LogInformation("Exiting GetAllBooks method with {Count} books fetched.", mappedBooks.Count());

                return new PaginatedResult<BookGetModel>
                {
                    TotalItems = paginatedBooks.TotalItems,
                    Items = mappedBooks,
                    TotalPages = paginatedBooks.TotalPages,
                    CurrentPage = paginatedBooks.CurrentPage,
                    HasPreviousPage = paginatedBooks.HasPreviousPage,
                    HasNextPage = paginatedBooks.HasNextPage,
                    FirstPage = paginatedBooks.FirstPage,
                    LastPage = paginatedBooks.LastPage
                };
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

                // Use AutoMapper to map from BookCreateModel to Book entity
                var bookEntity = _mapper.Map<Book>(bookModel);

                _context.Books.Add(bookEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while trying to create a book.");
                throw new Exception("Error while trying to create a book", ex);
            }
        }

        public async Task UpdateBook(int id, BookUpdateModel bookModel)
        {
            _logger.LogInformation($"Updating book with ID {id}.");
            var bookEntity = await _context.Books.FirstOrDefaultAsync(_ => _.Id == id);

            if (bookEntity == null)
            {
                _logger.LogWarning($"Book with ID {id} not found.");
                throw new KeyNotFoundException($"Book with ID {id} not found!");
            }

            // Use AutoMapper to map the updated fields from BookUpdateModel to the existing Book entity
            _mapper.Map(bookModel, bookEntity);

            _context.Books.Update(bookEntity);
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

        public async Task<IEnumerable<BookGetModel>> GetBooksByPublicationYear(int publicationYear)
        {

            _logger.LogInformation("Fetching books filtered by publication year from the database.");

            // Query to filter books by the specified publication year
            var filteredBooks = await _context.Books
                .Include(b => b.Author)
                .Where(b => b.PublicationYear == publicationYear)
                .ToListAsync();

            if (filteredBooks == null)
            {
                _logger.LogWarning("Book with that publication year was not found!");
                throw new KeyNotFoundException("Book with that publication year not found!");
            }

            // Map the results
            var mappedBooks = _mapper.Map<IEnumerable<BookGetModel>>(filteredBooks);

            _logger.LogInformation("Exiting GetBooksByPublicationYear method with {Count} books fetched.", mappedBooks.Count());

            return mappedBooks;
        }

        public async Task<IEnumerable<BookGetModel>> GetBooksByLanguage(string language)
        {
            _logger.LogInformation("Fetching all books by language!");
            var filteredBooks = await _context.Books
                .Include(x => x.Author)
                .Where(b => b.Language.Equals(language))
                .ToListAsync();

            if (filteredBooks == null)
            {
                _logger.LogWarning("Book/s with that language was not found!");
                throw new KeyNotFoundException("Book/s with that language was not found!");
            }

            var mappedBooks = _mapper.Map<IEnumerable<BookGetModel>>(filteredBooks);
            _logger.LogInformation("All books by language fetched!");
            return mappedBooks;
        }

        public async Task<IEnumerable<BookGetModel>> SelectBooksByGenres(string[] genres)
        {
            var filteredBooks = await _context.Books
                .Include(x => x.Author)
                .Where(x => genres.Any(genre => x.Genres.Contains(genre)))
                .ToListAsync();

            var mappedBooks = _mapper.Map<IEnumerable<BookGetModel>>(filteredBooks);
            return mappedBooks;
        }

        public async Task<IEnumerable<BookGetModel>> SearchBook(string book)
        {
            if (string.IsNullOrWhiteSpace(book))
            {
                return Enumerable.Empty<BookGetModel>();
            }


            book = book.ToLower();

            var searchedBooks = await _context.Books
                .Include(x => x.Author)
                .Where(b => b.Title.ToLower().Contains(book))
                .ToListAsync();

            var mappedBooks = _mapper.Map<IEnumerable<BookGetModel>>(searchedBooks);
            return mappedBooks;
        }

        public async Task<IEnumerable<BookGetModel>> GetBooksByAuthorName(string authorName)
        {
            if(string.IsNullOrWhiteSpace(authorName))
            {
                _logger.LogWarning("Author name cant be null or empty!");
                throw new ArgumentException("Author name cant be null or empty!");
            }

            _logger.LogInformation("Fetching books by author name");

            var author = await _context.Authors
                .FirstOrDefaultAsync(a => a.Name.ToLower().Contains(authorName.ToLower())); 

            if(author == null)
            {
                _logger.LogWarning($"No author found with name: {authorName}");
                throw new KeyNotFoundException($"No author found with name: {authorName}");
            }

            var books = await _context.Books
                .Include(b => b.Author)
                .Where(b => b.AuthorId == author.Id)
                .ToListAsync();

            if(books.Count == 0)
            {
                _logger.LogWarning($"No books found for author: {authorName}");
                throw new KeyNotFoundException($"No books found for author: {authorName}");
            }

            var mappedBooks = _mapper.Map<IEnumerable<BookGetModel>>(books);
            return mappedBooks; 
        }
    }

}

