
using Sales.Domain.Errors;
using Sales.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Sales.Core.Helpers;

public static class ResultHelper
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.Match(
        (value) => new OkObjectResult(value),
        ErrorToActionResult
        );
    }

    public static IActionResult ErrorToActionResult(this Error error)
    {
        return error switch
        {
            BadRequestError => new BadRequestObjectResult(error.Content),
            InternalServerError => new ObjectResult(error.Content)
            {
                StatusCode = 500
            },
            _ => new ObjectResult(InternalServerError.DEFAULT_INTERNAL_SERVER_ERROR_MESSAGE)
            {
                StatusCode = 500
            },
        };
    }
}