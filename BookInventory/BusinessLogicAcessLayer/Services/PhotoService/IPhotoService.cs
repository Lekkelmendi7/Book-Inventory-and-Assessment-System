using BookInventory.DataAccessLayer.Entities;

namespace BookInventory.BusinessLogicAcessLayer.Services.PhotoService
{
    public interface IPhotoService
    {
        Task<IEnumerable<BookPhoto>> GetAllAsync();
        Task<BookPhoto?> GetByIdAsync(string id);
        Task<BookPhoto> AddAsync(BookPhoto bookPhoto);
        Task<bool> UpdateAsync(BookPhoto bookPhoto);
        Task<bool> DeleteByIdAsync(string id);

        // Updated methods for book photo management
        Task<string?> GetBookPhotoUrl(int bookId);
        Task<string?> GetBookPhotoById(int bookId);
        Task AddBookPhoto(int bookId, string photoUrl, string photoName);
        Task UpdateBookPhoto(int bookId, string newPhotoUrl, string newPhotoName);
        Task DeleteBookPhotoByBookIdAsync(int bookId);
    }
}
