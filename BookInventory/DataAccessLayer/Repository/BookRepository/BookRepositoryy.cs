using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.BusinessLogicAcessLayer.Models;
using BookInventory.DataAccess.Database;
using BookInventory.DataAccess.Entities;
using BookInventory.DataAccessLayer.Repository.Repo;
using BookInventory.LogicAcessLayer.Models.BookModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookInventory.DataAccessLayer.Repository.BookRepository
{
    public class BookRepositoryy : Repository<Book>, IBookRepository
    {
        private readonly DbSet<Book> _dbSet;

        public BookRepositoryy(DatabaseContext context) : base(context)
        {
            _dbSet = context.Set<Book>();
        }

        public async Task<PaginatedResult<Book>> GetAllBooks(int page, int size, string? sortBy = null, string? sortOrder = null)
        {
            var queryable = _dbSet.AsQueryable();

            // Sorting logic
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                queryable = sortOrder?.ToLower() == "desc"
                    ? sortBy switch
                    {
                        "title" => queryable.OrderByDescending(b => b.Title),
                        "publicationyear" => queryable.OrderByDescending(b => b.PublicationYear),
                        "language" => queryable.OrderByDescending(b => b.Language),
                        "price" => queryable.OrderByDescending(b => b.Price),
                        "pagecount" => queryable.OrderByDescending(b => b.PageCount),
                        "stock" => queryable.OrderByDescending(b => b.Stock),
                        "edition" => queryable.OrderByDescending(b => b.Edition),
                        "format" => queryable.OrderByDescending(b => b.Format),
                        _ => queryable.OrderByDescending(b => b.Title),
                    }
                    : sortBy switch
                    {
                        "title" => queryable.OrderBy(b => b.Title),
                        "publicationyear" => queryable.OrderBy(b => b.PublicationYear),
                        "language" => queryable.OrderBy(b => b.Language),
                        "price" => queryable.OrderBy(b => b.Price),
                        "pagecount" => queryable.OrderBy(b => b.PageCount),
                        "stock" => queryable.OrderBy(b => b.Stock),
                        "edition" => queryable.OrderBy(b => b.Edition),
                        "format" => queryable.OrderBy(b => b.Format),
                        _ => queryable.OrderBy(b => b.Title),
                    };
            }
            else
            {
                queryable = queryable.OrderBy(b => b.Title); // Default sorting
            }

            // Paginate
            return await GetPaginatedAsync(page, size, q => queryable); // Wrap `queryable` in a lambda function
        }


        public async Task<Book> GetBookById(int id)
        {
            return await GetByIdAsync(id);
        }

        public async Task AddBook(Book book)
        {
            await AddAsync(book);
        }

        public Task UpdateBook(Book book)
        {
            Update(book);
            return Task.CompletedTask;
        }

        public Task DeleteBook(Book book)
        {
            Delete(book);
            return Task.CompletedTask;
        }


        public async Task<IEnumerable<Book>> GetBooksByPublicationYearAsync(int? publicationYear)
        {
            if (!publicationYear.HasValue)
            {
                return Enumerable.Empty<Book>();
            }

            return await _dbSet.Where(b => b.PublicationYear == publicationYear.Value).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByLanguageAsync(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                return Enumerable.Empty<Book>();
            }

            try
            {
                // Fetch books from the database where the language matches (case-insensitive)
                var books = await _dbSet
                    .Where(b => EF.Functions.Like(b.Language, $"%{language}%")) // Case-insensitive search
                    .ToListAsync();

                return books;
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error fetching books by language");
                throw new InvalidOperationException("An error occurred while retrieving books by language.", ex);
            }
        }





        public async Task<IEnumerable<Book>> SelectBooksByGenresAsync(string[] genres)
        {
            if (genres == null || !genres.Any())
            {
                return Enumerable.Empty<Book>();
            }

            return await _dbSet.Where(b => b.Genres.Any(g => genres.Contains(g))).ToListAsync();
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Enumerable.Empty<Book>();
            }

            // Case-insensitive search for title and author's name
            return await _dbSet
                .AsNoTracking() // No need to track entities since this is a read-only operation
                .Where(b => EF.Functions.Like(b.Title, $"%{searchTerm}%") ||
                            EF.Functions.Like(b.Author.Name, $"%{searchTerm}%")) // Assuming Author has a Name property
                .ToListAsync();
        }


        public async Task<IEnumerable<Book>> GetBooksByAuthorNameAsync(string authorName)
        {
            if (string.IsNullOrWhiteSpace(authorName))
            {
                return Enumerable.Empty<Book>();
            }

            // Use EF.Functions.Like for case-insensitive exact match on author's name
            return await _dbSet
                .AsNoTracking()
                .Where(b => EF.Functions.Like(b.Author.Name, $"%{authorName}%")) // Assuming Author has a Name property
                .ToListAsync();
        }


    }
}
