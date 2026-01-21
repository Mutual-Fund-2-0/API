using System.Text.Json;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// REST API controller for mutual fund scheme operations.
/// </summary>
/// <param name="logger">Logger instance for HTTP request</param>
/// <param name="service">Service for mutual fund operations</param>
[ApiController]
[Route("[controller]")]
public class MutualFundController(ILogger<MutualFundController> logger, IMutualFundService service) : ControllerBase
{

    /// <summary>
    /// Logger instance for HTTP request/response lifecycle.
    /// </summary>
    private readonly ILogger<MutualFundController> _logger = logger;

    /// <summary>
    /// Handles mutual fund scheme data processing.
    /// </summary>
    private readonly IMutualFundService _service = service;

    /// <summary>
    /// EventId for HTTP request start events.
    /// </summary>
    private static readonly EventId RequestStart = new(3000, "RequestStart");

    /// <summary>
    /// EventId for successful HTTP responses.
    /// </summary>
    private static readonly EventId RequestSuccess = new(3001, "RequestSuccess");

    /// <summary>
    /// EventId for service layer failures in HTTP context.
    /// </summary>
    private static readonly EventId ServiceError = new(3002, "ServiceError");

    /// <summary>
    /// GET endpoint returning mutual fund schemes.
    /// </summary>
    /// <param name="pageNumber">Current page number</param>
    /// <returns>JSON object containing mutual fund schemes and correlation ID</returns>
    /// <response code="200">Returns mutual fund schemes</response>
    /// <response code="500">Internal server error with correlation ID</response>
    [HttpGet("schemes", Name = "GetMutualFundSchemes")]
    public async Task<IActionResult> GetMutualFundSchemesAsync(int pageNumber = 1)
    {
        var correlationId = HttpContext.TraceIdentifier;
        _logger.LogDebug(RequestStart, "HTTP GET /schemes [{CorrelationId}]", correlationId);
        try
        {
            var page = await _service.GetMutualFundSchemesAsync(pageNumber);
            _logger.LogInformation(RequestSuccess, "HTTP GET /schemes/count [{CorrelationId}] returned {Page}", correlationId, JsonSerializer.Serialize(page));
            return Ok(new { page, correlationId });
        }
        catch(Exception e)
        {
            _logger.LogError(ServiceError, e, "HTTP GET /schemes [{CorrelationId}] {repository}-{Method}: Failed", correlationId, nameof(MutualFundController), nameof(GetMutualFundSchemesAsync));
            return StatusCode(500, new { Error = "Internal server error", CorrelationId = correlationId });
        }
    }
}