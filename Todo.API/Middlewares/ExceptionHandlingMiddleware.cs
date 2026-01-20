namespace Todo.API.Middlewares;

#pragma warning disable CS1591

public class ExceptionHandlingMiddleware
{
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error detected by middleware");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        ApiResponse<string> response;
        int statusCode = 0;

        switch (exception)
        {
            case BusinessException businessException:
                response = new ApiResponse<string>(businessException.Message, "Violation of business rule", false);
                statusCode = (int)HttpStatusCode.BadRequest;
                break;

            default:
                response = new ApiResponse<string>($"An unexpected error occurred. Details: {exception.Message}", "Internal server error.", false);
                statusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

#pragma warning restore CS1591
