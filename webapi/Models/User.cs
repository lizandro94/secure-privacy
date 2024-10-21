using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace webapi.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("UserName")]
        public required string UserName { get; set; }
        [BsonElement("Password")]
        public required string Password { get; set; }
        [BsonElement("FirstName")]
        public string? FirstName { get; set; }
        [BsonElement("LastName")]
        public string? LastName { get; set; }
        public List<Product>? Products { get; set; }
    }
}