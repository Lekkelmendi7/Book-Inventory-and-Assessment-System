using AutoMapper;
using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.BusinessLogicAcessLayer.Models;
using BookInventory.BusinessLogicAcessLayer.Models.BookModels;
using BookInventory.BusinessLogicAcessLayer.Services.PhotoService;
using BookInventory.DataAccess.Database;
using BookInventory.DataAccess.Entities;
using BookInventory.DataAccessLayer.Entities;
using BookInventory.LogicAcessLayer.Models.BookModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookInventory.LogicAcessLayer.Services.BookService
{
    public class BookServicee : IBookService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly ILogger<BookServicee> _logger;

        public BookServicee(DatabaseContext context, IMapper mapper, ILogger<BookServicee> logger, IPhotoService photoService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _photoService = photoService;
        }

        public async Task<PaginatedResult<BookGetModel>> GetAllBooks(int page, int size, string? sortBy, string? sortOrder)
        {
            size = Math.Min(size, PaginationModel.MaxPageSize);

            try
            {
                _logger.LogInformation("Fetching books with pagination: page {Page}, size {Size}.", page, size);

                var queryable = _context.Books.Include(b => b.Author).AsQueryable();
                
                if(!string.IsNullOrWhiteSpace(sortBy))
                {
                    switch(sortBy)
                    {
                        case "title":
                            queryable = sortOrder == "desc" ? queryable.OrderByDescending(a => a.Title) : queryable.OrderBy(a => a.Title);
                            break;
                        case "publicationYear":
                            queryable = sortOrder == "desc" ? queryable.OrderByDescending(a => a.PublicationYear) : queryable.OrderBy(a => a.PublicationYear);
                            break;
                        case "language":
                            queryable = sortOrder == "desc" ? queryable.OrderByDescending(a => a.Language) : queryable.OrderBy(a => a.Language);
                            break;
                        case "price":
                            queryable = sortOrder == "desc" ? queryable.OrderByDescending(a => a.Price) : queryable.OrderBy(a => a.Price);
                            break;
                        case "pageCount":
                            queryable = sortOrder == "desc" ? queryable.OrderByDescending(a => a.PageCount) : queryable.OrderBy(a => a.PageCount);
                            break;
                        case "stock":
                            queryable = sortOrder == "desc" ? queryable.OrderByDescending(a => a.Stock) : queryable.OrderBy(a => a.Stock);
                            break;
                        case "edition":
                            queryable = sortOrder == "desc" ? queryable.OrderByDescending(a => a.Edition) : queryable.OrderBy(a => a.Edition);
                            break;
                        case "format":
                            queryable = sortOrder == "desc" ? queryable.OrderByDescending(a => a.Format) : queryable.OrderBy(a => a.Format);
                            break;
                        default:
                            queryable = queryable.OrderBy(a => a.Title); // Default sorting
                            break;
                    }
                }
                
                var paginatedBooks = queryable.Paginate(page, size);
                var mappedBooks = _mapper.Map<IEnumerable<BookGetModel>>(paginatedBooks.Items);

                foreach (var book in mappedBooks)
                {
                    book.BookPhoto = await _photoService.GetBookPhotoById(book.Id); // Get photo name
                    book.BookPhotoUrl = await _photoService.GetBookPhotoUrl(book.Id); // Get photo URL
                }

                _logger.LogInformation("Fetched {Count} books successfully.", mappedBooks.Count());

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
                _logger.LogError(ex, "Error fetching books.");
                throw new Exception("Error fetching books.", ex);
            }
        }

        public async Task<BookGetModel> GetBookById(int id)
        {
            _logger.LogInformation("Fetching book with ID {Id}.", id);

            var book = await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                _logger.LogWarning("Book with ID {Id} not found.", id);
                throw new KeyNotFoundException($"Book with ID {id} not found.");
            }

            var bookModel = _mapper.Map<BookGetModel>(book);
            bookModel.BookPhoto = await _photoService.GetBookPhotoById(book.Id);
            bookModel.BookPhotoUrl = await _photoService.GetBookPhotoUrl(book.Id);

            return bookModel;
        }

        public async Task CreateBook(BookCreateModel bookModel, string? newPhotoUrl = null, string? initialPhoto = null)
        {
            try
            {
                _logger.LogInformation("Creating a new book.");

                var bookEntity = _mapper.Map<Book>(bookModel);
                bookEntity.BookPhoto = string.IsNullOrWhiteSpace(initialPhoto) ? "NoPhoto.png" : initialPhoto;

                _context.Books.Add(bookEntity);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrWhiteSpace(initialPhoto))
                {
                    await _photoService.AddBookPhoto(bookEntity.Id, newPhotoUrl, initialPhoto);
                }

                _logger.LogInformation("Book with ID {Id} created successfully.", bookEntity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating book.");
                throw new Exception("Error creating book.", ex);
            }
        }

        public async Task UpdateBook(int id, BookUpdateModel bookModel, string? newPhotoUrl = null, string? newPhotoName = null)
        {
            _logger.LogInformation("Updating book with ID {Id}.", id);

            var bookEntity = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (bookEntity == null)
            {
                _logger.LogWarning("Book with ID {Id} not found.", id);
                throw new KeyNotFoundException($"Book with ID {id} not found.");
            }

            _mapper.Map(bookModel, bookEntity);

            if (!string.IsNullOrWhiteSpace(newPhotoName))
            {
                await _photoService.UpdateBookPhoto(id, newPhotoUrl ,newPhotoName);
            }

            _context.Books.Update(bookEntity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Book with ID {Id} updated successfully.", id);
        }


        public async Task<bool> DeleteBook(int bookId)
        {
            try
            {
                _logger.LogInformation("Deleting book with ID {Id}.", bookId);

                // Gjej librin nëse ekziston
                var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId);
                if (book == null)
                {
                    _logger.LogWarning("Book with ID {Id} not found.", bookId);
                    return false; // Libri nuk u gjet
                }

                // Fshi foton e librit nëse ekziston
                await _photoService.DeleteBookPhotoByBookIdAsync(bookId);

                // Fshi librin nga baza e të dhënave
                _context.Books.Remove(book);
                await _context.SaveChangesAsync(); // Sigurohu që të dhënat të ruhen

                _logger.LogInformation("Book with ID {Id} and its photo deleted successfully.", bookId);
                return true; // Libri dhe fotot janë fshirë me sukses
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book with ID {Id}.", bookId);
                throw new Exception("Error deleting book.", ex);
            }
        }


        public async Task<IEnumerable<BookGetModel>> GetBooksByPublicationYear(int? publicationYear)
        {
            _logger.LogInformation("Fetching books published in year {Year}.", publicationYear);

            var books = await _context.Books
                .Where(b => publicationYear == null || b.PublicationYear == publicationYear)
                .Include(b => b.Author)
                .ToListAsync();

            var bookModels = _mapper.Map<IEnumerable<BookGetModel>>(books);
            foreach (var book in bookModels)
            {
                book.BookPhoto = await _photoService.GetBookPhotoById(book.Id);
            }

            return bookModels;
        }

        public async Task<IEnumerable<BookGetModel>> GetBooksByLanguage(string language)
        {
            _logger.LogInformation("Fetching books in language {Language}.", language);

            var books = await _context.Books
                .Where(b => b.Language == language)
                .Include(b => b.Author)
                .ToListAsync();

            var bookModels = _mapper.Map<IEnumerable<BookGetModel>>(books);
            foreach (var book in bookModels)
            {
                book.BookPhoto = await _photoService.GetBookPhotoById(book.Id);
            }

            return bookModels;
        }

        public async Task<IEnumerable<BookGetModel>> SelectBooksByGenres(string[] genres)
        {
            _logger.LogInformation("Fetching books with genres {Genres}.", string.Join(", ", genres));

            var books = await _context.Books
                .Where(b => b.Genres.Any(g => genres.Contains(g)))
                .Include(b => b.Author)
                .ToListAsync();

            var bookModels = _mapper.Map<IEnumerable<BookGetModel>>(books);
            foreach (var book in bookModels)
            {
                book.BookPhoto = await _photoService.GetBookPhotoById(book.Id);
            }

            return bookModels;
        }

        public async Task<IEnumerable<BookGetModel>> SearchBook(string searchTerm)
        {
            _logger.LogInformation("Searching for books with term {Term}.", searchTerm);

            var books = await _context.Books
                .Where(b => b.Title.Contains(searchTerm) || b.Author.Name.Contains(searchTerm))
                .Include(b => b.Author)
                .ToListAsync();

            var bookModels = _mapper.Map<IEnumerable<BookGetModel>>(books);
            foreach (var bookModel in bookModels)
            {
                bookModel.BookPhoto = await _photoService.GetBookPhotoById(bookModel.Id);
            }

            return bookModels;
        }


        public async Task<IEnumerable<BookGetModel>> GetBooksByAuthorName(string authorName)
        {
            _logger.LogInformation("Fetching books by author {AuthorName}.", authorName);

            var books = await _context.Books
                .Where(b => b.Author.Name == authorName)
                .Include(b => b.Author)
                .ToListAsync();

            var bookModels = _mapper.Map<IEnumerable<BookGetModel>>(books);
            foreach (var book in bookModels)
            {
                book.BookPhoto = await _photoService.GetBookPhotoById(book.Id);
            }

            return bookModels;
        }
    }
}
