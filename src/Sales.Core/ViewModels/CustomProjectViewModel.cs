
using System.Text.Json.Serialization;
using Sales.Domain.Entities;

namespace Sales.Core.ViewModels;

public class CustomProjectViewModel : ProjectViewModel
{
    [JsonPropertyName("filter")]
    public required Filter Filter { get; set; }
}
