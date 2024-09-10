using BookInventory.DataAccessLayer.Database;
using BookInventory.DataAccessLayer.Entities;
using MongoDB.Driver;
using ZstdSharp.Unsafe;

namespace BookInventory.BusinessLogicAcessLayer.Services.PhotoService
{
    public class PhotoService : IPhotoService
    {
        private readonly IMongoCollection<BookPhoto> _bookphotos;

        public PhotoService(MongoDbService mongoDbService)
        {
            _bookphotos = mongoDbService.Database?.GetCollection<BookPhoto>("bookphoto")
                ?? throw new ArgumentNullException(nameof(mongoDbService.Database));
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id cannot be null or empty");

            var result = await _bookphotos.DeleteOneAsync(Builders<BookPhoto>.Filter.Eq(x => x.Id, id));
            return result.DeletedCount > 0;
        }
        public async Task<IEnumerable<BookPhoto>> GetAllAsync()
        {
            return await _bookphotos.Find(FilterDefinition<BookPhoto>.Empty).ToListAsync();
        }


        public async Task<BookPhoto?> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id cannot be null or empty");

            return await _bookphotos.Find(Builders<BookPhoto>.Filter.Eq(x => x.Id, id)).FirstOrDefaultAsync();
        }

        public Task<BookPhoto> AddAsync(BookPhoto bookPhoto)
        {
            if (bookPhoto == null) throw new ArgumentNullException(nameof(bookPhoto));

            return _bookphotos.InsertOneAsync(bookPhoto).ContinueWith(_ => bookPhoto);
        }

        public async Task<bool> UpdateAsync(BookPhoto bookPhoto)
        {
            if (bookPhoto == null || string.IsNullOrWhiteSpace(bookPhoto.Id))
                throw new ArgumentException("Invalid bookPhoto object or missing Id");

            var result = await _bookphotos.ReplaceOneAsync(Builders<BookPhoto>.Filter.Eq(x => x.Id, bookPhoto.Id), bookPhoto);
            return result.MatchedCount > 0;
        }

        public async Task<string?> GetBookPhotoUrl(int bookId) =>
            (await _bookphotos.Find(Builders<BookPhoto>.Filter.Eq(x => x.BookId, bookId)).FirstOrDefaultAsync())?.PhotoUrl;

        public async Task<string?> GetBookPhotoById(int bookId) =>
            (await _bookphotos.Find(Builders<BookPhoto>.Filter.Eq(x => x.BookId, bookId)).FirstOrDefaultAsync())?.PhotosName;

        public Task AddBookPhoto(int bookId, string photoUrl, string photoName) =>
            AddAsync(new BookPhoto { BookId = bookId, PhotoUrl = photoUrl, PhotosName = photoName });

        public Task UpdateBookPhoto(int bookId, string newPhotoUrl, string newPhotoName) =>
            _bookphotos.UpdateOneAsync(
                Builders<BookPhoto>.Filter.Eq(x => x.BookId, bookId),
                Builders<BookPhoto>.Update.Set(x => x.PhotoUrl, newPhotoUrl).Set(x => x.PhotosName, newPhotoName)
            );

        public Task DeleteBookPhotoByBookIdAsync(int bookId) =>
    _bookphotos.DeleteOneAsync(Builders<BookPhoto>.Filter.Eq(x => x.BookId, bookId));   

    }
}
