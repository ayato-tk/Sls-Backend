using System.Text.Json.Serialization;

namespace Sales.Domain.Entities;

public class Cnae
{ 
    [JsonPropertyName("code")]
    public required string Code { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }
}
