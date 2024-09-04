using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.BusinessLogicAcessLayer.Models.BookModels;
using BookInventory.LogicAcessLayer.Models.BookModels;
using System.Numerics;

namespace BookInventory.LogicAcessLayer.Services.BookService
{
    public interface IBookService
    {
        Task<PaginatedResult<BookGetModel>> GetAllBooks(int page, int result);
        Task<BookGetModel> GetBookById(int id); 
        Task CreateBook(BookCreateModel bookModel);
        Task UpdateBook(int id, BookUpdateModel bookModel);
        Task<bool> DeleteBook(int id);
        Task<IEnumerable<BookGetModel>> GetBooksByPublicationYear(int publicationYear);
        Task<IEnumerable<BookGetModel>> GetBooksByLanguage(string language);
        Task<IEnumerable<BookGetModel>> SelectBooksByGenres(string[] genres);
        Task<IEnumerable<BookGetModel>> SearchBook(string book);
        Task<IEnumerable<BookGetModel>> GetBooksByAuthorName(string authorName);
    }
}
