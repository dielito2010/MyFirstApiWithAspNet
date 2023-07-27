namespace MyFirstApiWithAspNet;

public class ErrorResult : IActionResult
{
    private readonly string _title;
    private readonly int _statusCode;

    public ErrorResult(string title, int statusCode)
    {
        _title = title;
        _statusCode = statusCode;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var problemDetails = new ProblemDetails
        {
            Title = _title,
            Status = _statusCode,
        };

        context.HttpContext.Response.StatusCode = _statusCode;
        context.HttpContext.Response.ContentType = "application/problem+json";

        await context.HttpContext.Response.WriteAsJsonAsync(problemDetails);
    }
}
