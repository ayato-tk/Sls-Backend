using System.Text.Json;
using Sales.Domain.Common;

namespace Sales.Domain.Errors;

public record InternalServerError : Error
{
    public const string DEFAULT_INTERNAL_SERVER_ERROR_MESSAGE = "Internal server error.";


    public InternalServerError(string reason, string content) : base(reason, JsonSerializer.Deserialize<object>(content))
    {
    }

    public static InternalServerError CreateInternalServerError(string reason, string content)
    {
        return new InternalServerError(reason, content);
    }

    public static InternalServerError CreateInternalServerError(string content)
    {
        return new InternalServerError(DEFAULT_INTERNAL_SERVER_ERROR_MESSAGE, content);
    }
}
