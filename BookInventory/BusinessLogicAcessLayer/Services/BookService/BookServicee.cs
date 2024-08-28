﻿using AutoMapper;
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

    }
}
