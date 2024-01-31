using Sales.Domain.Enums;

namespace Sales.Core.DTOs;

public class NotifierDTO
{
    public required int SessionId { get; set; }

    public string? WebhookNotify { get; set; }

    public required ProjectStatus Status { get; set; }

    public required WebhookIdentification WebhookIdentification { get; set; }

    public string? Detail { get; set; }

    public string? Token { get; set; }

    public string? UserId { get; set; }

    public string? UserEmail { get; set; }
}
