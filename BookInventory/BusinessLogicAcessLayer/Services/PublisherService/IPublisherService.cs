using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.BusinessLogicAcessLayer.Models.PulisherModels;
using BookInventory.DataAccessLayer.Entities;
using System.Globalization;

namespace BookInventory.BusinessLogicAcessLayer.Services.PublisherService
{
    public interface IPublisherService
    {
        Task<PaginatedResult<PublisherGetModel>> GetPublishers(int page, int size, string? sortBy, string? sortOrder);
        Task<PublisherGetModel> GetPublisher(int id);
        Task AddPublisher(PublisherCreateModel model);
        Task UpdatePublisher(int id, PublisherUpdateModel model);
        Task<bool> DeletePublisher(int id);   
    }
}
