using System.Diagnostics;

namespace API.Middlewares;

/// <summary>
/// Globally tracks and logs all incoming HTTP requests, their execution time, and response status.
/// </summary>
/// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
/// <param name="logger">The logger instance used to write request execution details.</param>
public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    /// <summary>
    /// Invokes the middleware, measures the execution time of the downstream request pipeline, and logs the result.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var watch = Stopwatch.StartNew();
        await next(context);
        watch.Stop();
        logger.LogDebug("HTTP {Method} {Path}{QueryString} responded {StatusCode} in {ElapsedMilliseconds}ms", context.Request.Method, context.Request.Path, context.Request.QueryString.Value, context.Response.StatusCode, watch.ElapsedMilliseconds);
    }
}