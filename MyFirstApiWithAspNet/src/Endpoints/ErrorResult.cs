namespace MyFirstApiWithAspNet.Endpoints;

public class ErrorResult : IActionResult
{
    private readonly string _title;
    private readonly int _statusCode;
    private readonly string _errorMessage;

    public ErrorResult(string title, int statusCode, string errorMessage)
    {
        _title = title;
        _statusCode = statusCode;
        _errorMessage = errorMessage;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var problemDetails = new ProblemDetails
        {
            Title = _title,
            Status = _statusCode,
            Detail = _errorMessage
        };

        context.HttpContext.Response.StatusCode = _statusCode;
        context.HttpContext.Response.ContentType = "application/problem+json";

        await context.HttpContext.Response.WriteAsJsonAsync(problemDetails);
    }
}
