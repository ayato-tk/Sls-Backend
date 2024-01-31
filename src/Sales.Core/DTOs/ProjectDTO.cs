using MongoDB.Bson;
using Sales.Domain.Entities;

namespace Sales.Core.DTOs;

public class ProjectDTO
{
       public required string UserEmail { get; set; }

       public required string UserId { get; set; }

       public string? Customer { get; set; }

       public string? Title { get; set; }

       public bool? Sample { get; set; }

       public Filter? Filter { get; set; }
}