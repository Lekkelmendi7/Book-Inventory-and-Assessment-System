using BookInventory.LogicAcessLayer.Models.BookModels;

namespace BookInventory.LogicAcessLayer.Services.BookService
{
    public interface IBookService
    {
        Task<IEnumerable<BookGetModel>> GetAllBooks();
        Task<BookGetModel> GetBookById(int id); 
        Task CreateBook(BookCreateModel bookModel);
        Task UpdateBook(int id, BookUpdateModel bookModel);
        Task<bool> DeleteBook(int id);
    }
}
