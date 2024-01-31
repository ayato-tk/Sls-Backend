namespace Sales.Core.DTOs;

public class DashConsumerDTO
{
    public int SessionId { get; set; }

    public int RowsLimit { get; set; }

    public string? Table { get; set; }

    public string? OrderBy { get; set; }

    public string? Order { get; set; }
}
