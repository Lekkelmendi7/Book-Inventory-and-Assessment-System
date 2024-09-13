using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.BusinessLogicAcessLayer.Models;
using BookInventory.BusinessLogicAcessLayer.Models.BookModels;
using BookInventory.LogicAcessLayer.Models.BookModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookInventory.LogicAcessLayer.Services.BookService
{
    public interface IBookService
    {
        Task<PaginatedResult<BookGetModel>> GetAllBooks(int page, int size, string? stringBy, string? sortOrder);
        Task<BookGetModel> GetBookById(int id);
        Task CreateBook(BookCreateModel bookModel, string? newPhotoUrl = null, string? initialPhoto = null);
        Task UpdateBook(int id, BookUpdateModel bookModel, string? newPhotoUrl = null, string? newPhotoName = null);
        Task<bool> DeleteBook(int id);
        Task<IEnumerable<BookGetModel>> GetBooksByPublicationYear(int? publicationYear);
        Task<IEnumerable<BookGetModel>> GetBooksByLanguage(string language);
        Task<IEnumerable<BookGetModel>> SelectBooksByGenres(string[] genres);
        Task<IEnumerable<BookGetModel>> SearchBook(string book);
        Task<IEnumerable<BookGetModel>> GetBooksByAuthorName(string authorName);


    }
}
