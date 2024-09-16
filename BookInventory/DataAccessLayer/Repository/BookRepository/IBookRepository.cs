using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.DataAccess.Entities;
using BookInventory.DataAccessLayer.Repository.Repo;

namespace BookInventory.DataAccessLayer.Repository.BookRepository
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<PaginatedResult<Book>> GetAllBooks(int page, int size, string? sortBy = null, string? sortOrder = null);
        Task<Book> GetBookById(int id);
        Task AddBook(Book book);
        Task UpdateBook(Book book); 
        Task DeleteBook(Book book);
        Task<IEnumerable<Book>> GetBooksByPublicationYearAsync(int? publicationYear);
        Task<IEnumerable<Book>> GetBooksByLanguageAsync(string language);
        Task<IEnumerable<Book>> SelectBooksByGenresAsync(string[] genres);
        Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm);
        Task<IEnumerable<Book>> GetBooksByAuthorNameAsync(string authorName);
    }
}
