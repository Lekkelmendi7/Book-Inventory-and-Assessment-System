using BookInventory.DataAccessLayer.Database;
using BookInventory.DataAccessLayer.Entities;
using MongoDB.Driver;

namespace BookInventory.BusinessLogicAcessLayer.Services.PhotoService
{
    public class PhotoService : IPhotoService
    {
        private readonly IMongoCollection<BookPhoto>? _bookphotos;

        public PhotoService(MongoDbService mongoDbService)
        {
            _bookphotos = mongoDbService.Database?.GetCollection<BookPhoto>("bookphoto");
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Id cannot be null or empty", nameof(id));
            }

            var filter = Builders<BookPhoto>.Filter.Eq(x => x.Id, id);
            var result = await _bookphotos.DeleteOneAsync(filter);

            return result.DeletedCount > 0; // Return true if deletion succeeded
        }

        public async Task<IEnumerable<BookPhoto>> GetAllAsync()
        {
            return await _bookphotos.Find(FilterDefinition<BookPhoto>.Empty).ToListAsync();
        }

        public async Task<BookPhoto?> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Id cannot be null or empty", nameof(id));
            }

            var filter = Builders<BookPhoto>.Filter.Eq(x => x.Id, id);
            return await _bookphotos.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<BookPhoto> AddAsync(BookPhoto bookPhoto)
        {
            if (bookPhoto == null)
            {
                throw new ArgumentNullException(nameof(bookPhoto), "BookPhoto cannot be null");
            }

            await _bookphotos.InsertOneAsync(bookPhoto);
            return bookPhoto; // Return the inserted entity
        }

        public async Task<bool> UpdateAsync(BookPhoto bookPhoto)
        {
            if (bookPhoto == null || string.IsNullOrEmpty(bookPhoto.Id))
            {
                throw new ArgumentException("Invalid bookPhoto object or missing Id");
            }

            var filter = Builders<BookPhoto>.Filter.Eq(x => x.Id, bookPhoto.Id);
            var result = await _bookphotos.ReplaceOneAsync(filter, bookPhoto);

            return result.MatchedCount > 0; // Return true if an update was made
        }
    }
}

