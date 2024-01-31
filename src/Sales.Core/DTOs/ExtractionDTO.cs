namespace Sales.Core.DTOs;

public class ExtractionDTO
{
    public int SessionId { get; set; }

    public int RowsLimit { get; set; }

    public string? WebhookNotify { get; set; }

    public required string Token { get; set; }
}
