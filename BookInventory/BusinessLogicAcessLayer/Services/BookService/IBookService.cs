using BookInventory.BusinessLogicAcessLayer.Helpers;
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
    }
}
