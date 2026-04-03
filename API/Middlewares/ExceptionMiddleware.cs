using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Middlewares;

/// <summary>
/// Intercepts unhandled exceptions across the application, logging the error 
/// and returning a standardized HTTP 500 ProblemDetails JSON response.
/// </summary>
/// <param name="logger">The logger instance used to record exception details.</param>
public class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger) : IExceptionHandler
{
    /// <summary>
    /// Attempts to handle the specified exception by writing a standardized error response to the client.
    /// </summary>
    /// <param name="httpContext">The HTTP context associated with the failed request.</param>
    /// <param name="exception">The unhandled exception that occurred during request processing.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns><c>true</c> indicating the exception was successfully handled and the response pipeline should short-circuit.</returns>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception caught by global handler: {Message}", exception.Message);
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Detail = "An unexpected error occurred while processing your request. Please try again later.",
            Instance = httpContext.Request.Path
        };
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true; 
    }
}
