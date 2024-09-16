using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.DataAccess.Database;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookInventory.DataAccessLayer.Repository.Repo
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<PaginatedResult<TEntity>> GetPaginatedAsync(int page, int size, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, string? sortBy = null, string? sortOrder = null)
        {
            var queryable = _dbSet.AsQueryable();

            // Apply includes
            if (include != null)
            {
                queryable = include(queryable);
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                sortBy = sortBy.Trim();
                var sortDirection = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase) ? "descending" : "ascending";
                var sortExpression = $"{sortBy} {sortDirection}";
                queryable = queryable.OrderBy(sortExpression);
            }
            else
            {
                // Default sorting if sortBy is not provided
                queryable = queryable.OrderBy("Id"); // Assuming 'Id' is a common property
            }

            var totalItems = await queryable.CountAsync();
            var items = await queryable
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return new PaginatedResult<TEntity>
            {
                TotalItems = totalItems,
                Items = items,
                TotalPages = (int)Math.Ceiling(totalItems / (double)size),
                CurrentPage = page,
                HasPreviousPage = page > 1,
                HasNextPage = page < (int)Math.Ceiling(totalItems / (double)size),
                FirstPage = 1,
                LastPage = (int)Math.Ceiling(totalItems / (double)size)
            };
        }
    }
}
