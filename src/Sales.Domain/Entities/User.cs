using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sales.Domain.Entities;

public class User
{
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id {  get; set; }

    [BsonElement("email")]
    public required string Email { get; set; }

    [BsonElement("password")]
    public required string Password { get; set; }
}
