using System.Text.Json;
using Sales.Domain.Common;

namespace Sales.Domain.Errors;

public record BadRequestError : Error
{
    public BadRequestError(string reason, string content) : base(reason, JsonSerializer.Deserialize<object>(content))
    {
    }

    public static BadRequestError CreateBadRequest(string reason, string content)
    {
        return new(reason, content);
    }
}

