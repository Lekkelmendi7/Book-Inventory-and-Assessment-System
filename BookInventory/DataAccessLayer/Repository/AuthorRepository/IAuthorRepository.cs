


using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.DataAccess.Entities;

namespace AuthorInventory.DataAccessLayer.Repository.AuthorRepository
{
    public interface IAuthorRepository
    {
        Task<PaginatedResult<Author>> GetAllAuthors(int page, int size, string? sortBy = null, string? sortOrder = null);
        Task<Author> GetAuthorById(int id);
        Task AddAuthor(Author author);
        Task UpdateAuthor(Author author);
        Task DeleteAuthor(Author author);
        Task<IEnumerable<Author>> GetAuthorsByNationalityAsync(string nationality);
    }
}
