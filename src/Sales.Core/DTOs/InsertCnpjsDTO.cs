namespace Sales.Core.DTOs;

public class InsertCnpjsDTO
{
    public int SessionId { get; set; }

    public int? RowsLimit { get; set; }

    public string? Token { get; set; }
}
