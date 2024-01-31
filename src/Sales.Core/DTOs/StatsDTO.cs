using MongoDB.Bson;
using Sales.Domain.Enums;

namespace Sales.Core.DTOs;

public class StatsDTO
{
    public int SessionId { get; set; }

    public required string UserId { get; set; }

    public string? Email { get; set; }

    public string? Token { get; set; }

    public int Limit { get; set; }

    public ProjectType ProjectType { get; set; } = ProjectType.Icp;
}
