using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;

namespace MyFirstApiWithAspNet;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (exception is SqlException sqlException)
        {
            if (sqlException.Number == -2)
            {
                var databaseTimeoutException = new DatabaseTimeoutException("Database connection timeout");
                return HandleCustomException(context, databaseTimeoutException, StatusCodes.Status503ServiceUnavailable, "503");
            }
            else if (sqlException.Number == 547)
            {
                var foreignKeyViolationException = new ForeignKeyViolationException("Foreign key constraint violation");
                return HandleCustomException(context, foreignKeyViolationException, StatusCodes.Status409Conflict, "409");
            }
            else
            {
                var databaseError = new DataBaseError("Database error");
                return HandleCustomException(context, databaseError, StatusCodes.Status500InternalServerError, "500");
            }
            // Outras verificações específicas de exceções relacionadas ao banco de dados, se necessário
        }
        else if (exception is UnauthorizedAccessException)
        {
            return HandleCustomException(
                context, exception, StatusCodes.Status401Unauthorized, "Unauthorized access - you don't have permission to perform this action.");
        }
        else if (exception is ArgumentException || exception is ValidationException)
        {
            return HandleCustomException(context, exception, StatusCodes.Status400BadRequest, "400");
        }
        else if (exception is NotFoundException)
        {
            return HandleCustomException(context, exception, StatusCodes.Status404NotFound, "404");
        }
        else if (exception is TooManyRequestsException)
        {
            return HandleCustomException(context, exception, StatusCodes.Status429TooManyRequests, "429");
        }

        return HandleCustomException(context, exception, StatusCodes.Status500InternalServerError, "500");
    }

    private Task HandleCustomException(HttpContext context, Exception exception, int statusCode, string errorMessage)
    {
        var problemDetails = new ProblemDetails
        {
            Title = exception.Message,
            Status = statusCode,
            Detail = errorMessage
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        return context.Response.WriteAsJsonAsync(problemDetails);
    }
}

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message)
    {
    }
}

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}

public class TooManyRequestsException : Exception
{
    public TooManyRequestsException(string message) : base(message)
    {
    }
}

public class DatabaseTimeoutException : Exception
{
    public DatabaseTimeoutException(string message) : base(message)
    {
    }
}

public class ForeignKeyViolationException : Exception
{
    public ForeignKeyViolationException(string message) : base(message)
    {
    }
}

public class DataBaseError : Exception
{
    public DataBaseError(string message) : base(message)
    {
    }
}

