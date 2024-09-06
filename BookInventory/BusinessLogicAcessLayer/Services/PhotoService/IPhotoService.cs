using BookInventory.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookInventory.BusinessLogicAcessLayer.Services.PhotoService
{
    public interface IPhotoService
    {
        Task<IEnumerable<BookPhoto>> GetAllAsync();
        Task<BookPhoto?> GetByIdAsync(string id);
        Task<BookPhoto> AddAsync(BookPhoto bookPhoto);
        Task<bool> UpdateAsync(BookPhoto bookPhoto);
        Task<bool> DeleteByIdAsync(string id);
    }
}
