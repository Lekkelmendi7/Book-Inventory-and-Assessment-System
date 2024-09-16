using AuthorInventory.DataAccessLayer.Repository.AuthorRepository;
using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.DataAccess.Database;
using BookInventory.DataAccess.Entities;
using BookInventory.DataAccessLayer.Repository.BookRepository;
using BookInventory.DataAccessLayer.Repository.Repo;
using Microsoft.EntityFrameworkCore;

namespace BookInventory.DataAccessLayer.Repository.AuthorRepository
{
    public class AuthorRepositoryy : Repository<Author>, IAuthorRepository
    {
        private readonly DbSet<Author> _dbSet;

        public AuthorRepositoryy(DatabaseContext context) : base(context)
        {
            _dbSet = context.Set<Author>();
        }

        public async Task AddAuthor(Author author)
        {
            await AddAsync(author);
        }

        public Task DeleteAuthor(Author author)
        {
            Delete(author);
            return Task.CompletedTask;
        }

        public async Task<PaginatedResult<Author>> GetAllAuthors(int page, int size, string? sortBy = null, string? sortOrder = null)
        {
            var queryable = _dbSet.AsQueryable();

            // Sorting logic
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                queryable = sortOrder?.ToLower() == "desc"
                    ? sortBy switch
                    {
                        "name" => queryable.OrderByDescending(b => b.Name),
                        "nationality" => queryable.OrderByDescending(b => b.Nationality),
                        "dateOfBirth" => queryable.OrderByDescending(b => b.DateOfBirth),
                        _ => queryable.OrderByDescending(b => b.Name),
                    }
                    : sortBy switch
                    {
                        "name" => queryable.OrderByDescending(b => b.Name),
                        "nationality" => queryable.OrderByDescending(b => b.Nationality),
                        "dateOfBirth" => queryable.OrderByDescending(b => b.DateOfBirth),
                        _ => queryable.OrderByDescending(b => b.Name),
                    };
            }
            else
            {
                queryable = queryable.OrderBy(b => b.Name); // Default sorting
            }

            // Paginate
            return await GetPaginatedAsync(page, size, q => queryable); // Wrap `queryable` in a lambda function
        }

        public async Task<Author> GetAuthorById(int id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<IEnumerable<Author>> GetAuthorsByNationalityAsync(string nationality)
        {
            if (string.IsNullOrWhiteSpace(nationality))
            {
                return Enumerable.Empty<Author>();
            }

            try
            {
                // Fetch books from the database where the language matches (case-insensitive)
                var authors = await _dbSet
                    .Where(b => EF.Functions.Like(b.Nationality, $"%{nationality}%")) // Case-insensitive search
                    .ToListAsync();

                return authors;
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error fetching books by language");
                throw new InvalidOperationException("An error occurred while retrieving books by language.", ex);
            }
        }

        public Task UpdateAuthor(Author author)
        {
            Update(author);
            return Task.CompletedTask;
        }
    }
}
