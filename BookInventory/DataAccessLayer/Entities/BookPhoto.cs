using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookInventory.DataAccessLayer.Entities
{
    public class BookPhoto
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Book_id"), BsonRepresentation(BsonType.Int32)]
        public int? BookId { get; set; }

        [BsonElement("PhotosName"), BsonRepresentation(BsonType.String)]
        public string? PhotosName { get; set; }

        [BsonElement("PhotoUrl"), BsonRepresentation(BsonType.String)]
        public string? PhotoUrl { get; set; }  // New property for the URL
    }
}
