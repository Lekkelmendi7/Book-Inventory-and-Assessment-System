using AutoMapper;
using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.BusinessLogicAcessLayer.Models;
using BookInventory.BusinessLogicAcessLayer.Models.BookModels;
using BookInventory.BusinessLogicAcessLayer.Services.PhotoService;
using BookInventory.DataAccess.Database;
using BookInventory.DataAccess.Entities;
using BookInventory.DataAccessLayer.Repository.BookRepository;
using BookInventory.LogicAcessLayer.Models.BookModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookInventory.LogicAcessLayer.Services.BookService
{
    public class BookServicee : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly ILogger<BookServicee> _logger;

        public BookServicee(IBookRepository bookRepository, IMapper mapper, ILogger<BookServicee> logger, IPhotoService photoService)
        {
            _bookRepository = bookRepository;
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

                // Call the repository method to get paginated and sorted results
                var paginatedBooks = await _bookRepository.GetAllBooks(page, size, sortBy, sortOrder);

                // Map the result using AutoMapper
                var mappedBooks = _mapper.Map<IEnumerable<BookGetModel>>(paginatedBooks.Items);

                // Fetch book photo URLs if needed
                foreach (var book in mappedBooks)
                {
                    book.BookPhoto = await _photoService.GetBookPhotoById(book.Id);
                    book.BookPhotoUrl = await _photoService.GetBookPhotoUrl(book.Id);
                }

                _logger.LogInformation("Fetched {Count} books successfully.", mappedBooks.Count());

                // Return paginated result
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

            var book = await _bookRepository.GetBookById(id);
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

                await _bookRepository.AddBook(bookEntity);

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

            var bookEntity = await _bookRepository.GetBookById(id);
            if (bookEntity == null)
            {
                _logger.LogWarning("Book with ID {Id} not found.", id);
                throw new KeyNotFoundException($"Book with ID {id} not found.");
            }

            _mapper.Map(bookModel, bookEntity);

            if (!string.IsNullOrWhiteSpace(newPhotoName))
            {
                await _photoService.UpdateBookPhoto(id, newPhotoUrl, newPhotoName);
            }

            await _bookRepository.UpdateBook(bookEntity);

            _logger.LogInformation("Book with ID {Id} updated successfully.", id);
        }

        public async Task<bool> DeleteBook(int bookId)
        {
            try
            {
                _logger.LogInformation("Deleting book with ID {Id}.", bookId);

                var book = await _bookRepository.GetBookById(bookId);
                if (book == null)
                {
                    _logger.LogWarning("Book with ID {Id} not found.", bookId);
                    return false;
                }

                await _photoService.DeleteBookPhotoByBookIdAsync(bookId);
                await _bookRepository.DeleteBook(book);

                _logger.LogInformation("Book with ID {Id} and its photo deleted successfully.", bookId);
                return true;
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

            var books = await _bookRepository.GetBooksByPublicationYearAsync(publicationYear);
            var bookModels = _mapper.Map<IEnumerable<BookGetModel>>(books);
            foreach (var book in bookModels)
            {
                book.BookPhoto = await _photoService.GetBookPhotoById(book.Id);
            }

            return bookModels;
        }

        public async Task<IEnumerable<BookGetModel>> GetBooksByLanguage(string language)
        {
            var books = await _bookRepository.GetBooksByLanguageAsync(language);
            var bookModels = _mapper.Map<IEnumerable<BookGetModel>>(books);

            foreach (var book in bookModels)
            {
                book.BookPhoto = await _photoService.GetBookPhotoById(book.Id);
            }

            return bookModels;
        }

        public async Task<IEnumerable<BookGetModel>> GetBooksByAuthor(string author)
        {
            _logger.LogInformation("Fetching books by author {Author}.", author);

            var books = await _bookRepository.GetBooksByAuthorNameAsync(author);
            var bookModels = _mapper.Map<IEnumerable<BookGetModel>>(books);
            foreach (var book in bookModels)
            {
                book.BookPhoto = await _photoService.GetBookPhotoById(book.Id);
            }

            return bookModels;
        }

        public async Task<IEnumerable<BookGetModel>> SearchBook(string book)
        {
            var books = await _bookRepository.SearchBooksAsync(book);
            var bookModels = _mapper.Map<IEnumerable<BookGetModel>>(books);
            foreach (var bookModel in bookModels)
            {
                bookModel.BookPhoto = await _photoService.GetBookPhotoById(bookModel.Id);
            }
            return bookModels;
        }

        public async Task<IEnumerable<BookGetModel>> GetBooksByAuthorName(string authorName)
        {
            var books = await _bookRepository.GetBooksByAuthorNameAsync(authorName);
            var bookModels = _mapper.Map<IEnumerable<BookGetModel>>(books);
            foreach (var bookModel in bookModels)
            {
                bookModel.BookPhoto = await _photoService.GetBookPhotoById(bookModel.Id);
            }
            return bookModels;
        }

        public async Task<IEnumerable<BookGetModel>> SelectBooksByGenres(string[] genres)
        {
            _logger.LogInformation("Fetching books by genres: {Genres}.", string.Join(", ", genres));

            try
            {
                // Fetch books from the repository
                var books = await _bookRepository.SelectBooksByGenresAsync(genres);
                if (books == null || !books.Any())
                {
                    _logger.LogInformation("No books found for genres: {Genres}.", string.Join(", ", genres));
                    return Enumerable.Empty<BookGetModel>();
                }

                // Map the book entities to book models
                var bookModels = _mapper.Map<IEnumerable<BookGetModel>>(books);

                // Fetch book photos and assign to book models
                foreach (var book in bookModels)
                {
                    book.BookPhoto = await _photoService.GetBookPhotoById(book.Id);
                }

                _logger.LogInformation("Fetched {Count} books successfully for genres: {Genres}.", bookModels.Count(), string.Join(", ", genres));

                return bookModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching books by genres: {Genres}.", string.Join(", ", genres));
                throw; // Re-throw the exception to let the caller handle it or propagate it
            }
        }

    }
}
