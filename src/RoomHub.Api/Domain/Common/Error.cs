using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RoomHub.Api.Domain.Common;

public enum ErrorType
{
    Validation,
    NotFound,
    Conflict
}

public record Error(string Code, string Message, ErrorType Type)
{
    public IResult ToProblem()
    {
        var statusCode = Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = Code,
            Detail = Message
        };
        problem.Extensions["code"] = Code;

        return Results.Problem(problem);
    }
}
