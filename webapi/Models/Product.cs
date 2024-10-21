using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace webapi.Models
{
    public class Product
    {
        [BsonElement("Name")]
        public required string Name { get; set; }
        public int Quantity { get; set; }
    }
}