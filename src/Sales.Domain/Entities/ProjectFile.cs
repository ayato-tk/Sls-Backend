using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Sales.Domain.Enums;

namespace Sales.Domain.Entities;

public class ProjectFile : Project
{
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }

    [BsonElement("UpdatedAt")]
    public string? UpdatedAt { get; set; }

    [BsonElement("startedAt")]
    public string? StartedAt { get; set; }

    [BsonElement("originalName")]
    public required string OriginalName { get; set; }

    [BsonElement("filename")]
    public required string Filename { get; set; }

    [BsonElement("count")]
    public int Count { get; set; }

    [BsonElement("statusCount")]
    public int StatusCount { get; set; }

    [BsonElement("origin")]
    public string Origin { get; set; } = FileOrigin.Origin[ProjectType.Icp];

    [BsonElement("userId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string UserId { get; set; } 

    [BsonElement("errorDetail")]
    public string? ErrorDetail { get; set; }

}