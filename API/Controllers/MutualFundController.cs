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
    /// GET endpoint returning mutual fund schemes.
    /// </summary>
    /// <param name="pageNumber">Current page number</param>
    /// <returns>JSON object containing mutual fund schemes and correlation ID</returns>
    /// <response code="200">Returns mutual fund schemes</response>
    /// <response code="500">Internal server error with correlation ID</response>
    [HttpGet("schemes", Name = "GetMutualFundSchemes")]
    public async Task<IActionResult> GetMutualFundSchemesAsync([FromQuery] int pageNumber)
    {
        try
        {
            var page = await _service.GetMutualFundSchemesAsync(pageNumber);
            _logger.LogInformation("HTTP GET /schemes/count returned {Page}", JsonSerializer.Serialize(page));
            return Ok(page);
        }
        catch(Exception e)
        {
            _logger.LogError(e, "HTTP GET /schemes {repository}-{Method}: Failed", nameof(MutualFundController), nameof(GetMutualFundSchemesAsync));
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }
}