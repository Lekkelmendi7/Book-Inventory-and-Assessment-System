using BookInventory.BusinessLogicAcessLayer.Helpers;

namespace BookInventory.DataAccessLayer.Repository.Repo
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<PaginatedResult<TEntity>> GetPaginatedAsync(int page, int size, Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null, string? sortBy = null, string? sortOrder = null);
        Task<TEntity> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task SaveAsync();
    }

}
