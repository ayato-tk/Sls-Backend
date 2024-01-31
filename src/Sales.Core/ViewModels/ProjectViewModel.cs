using System;
using System.Text.Json.Serialization;
using Sales.Domain.Entities;

namespace Sales.Core.ViewModels;

public class ProjectViewModel
{
    [JsonPropertyName("sessionId")]
    public int? SessionId { get; set; }

    [JsonPropertyName("sessionGuid")]
    public required string SessionGuid { get; set; }

    [JsonPropertyName("userEmail")]
    public required string UserEmail { get; set; }

     [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = "CREATED";

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("origin")]
    public required string Origin { get; set; }

    [JsonPropertyName("countsResponse")]
    public Counts? CountsResponse { get; set; }

    [JsonPropertyName("dashResponse")]
    public Dashboard? DashResponse { get; set; }
}
