using System;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sales.Domain.Entities;

public class Project
{
    [JsonPropertyName("sessionId")]
    [BsonElement("sessionId")]
    public int SessionId { get; set; }

    [JsonPropertyName("sessionGuid")]
    [BsonElement("sessionGuid")]
    public string? SessionGuid { get; set; }

    [JsonPropertyName("userEmail")]
    [BsonElement("userEmail")]
    public required string UserEmail { get; set; }
    
    [JsonPropertyName("createdAt")]
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("customer")]
    [BsonIgnoreIfNull]
    public string? Customer { get; set; }
    
    [BsonElement("title")]
    public string Title { get; set; } = "";
    
    [BsonElement("sample")]
    [BsonIgnoreIfNull]
    public bool? Sample { get; set; }

    [BsonElement("status")]
    public string? Status { get; set; }

    [BsonElement("countsResponse")]
    public dynamic? CountsResponse { get; set; }

    [BsonElement("dashResponse")]
    public dynamic? DashResponse { get; set; }

    [BsonElement("potentialCounts")]
    public dynamic? PotentialCounts { get; set; }

    [JsonPropertyName("filter")]
    [BsonElement("filter")]
    [BsonIgnoreIfNull]
    public Filter? Filter { get; set; }
}
