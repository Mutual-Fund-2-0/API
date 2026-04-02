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
    /// GET endpoint returning mutual fund schemes with search and pagination.
    /// </summary>
    /// <param name="pageNumber">The current page index.</param>
    /// <param name="pageSize">Number of schemes per page.</param>
    /// <param name="searchText">Optional text to filter schemes by name or category</param>
    /// <returns>A paginated list of mutual fund schemes</returns>
    /// <response code="200">Returns mutual fund schemes</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("schemes", Name = "GetMutualFundSchemes")]
    public async Task<IActionResult> GetMutualFundSchemesAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? searchText = null)
    {
        _logger.LogInformation("HTTP GET /schemes | Search: {Search}", searchText);
        try
        {
            var page = await _service.GetMutualFundSchemesAsync(pageNumber, pageSize, searchText);
            _logger.LogDebug("HTTP GET /schemes returned {Page}", JsonSerializer.Serialize(page));
            return Ok(page);
        }
        catch(Exception e)
        {
            _logger.LogError(e, "HTTP GET /schemes {Controller} - {Action}: Failed", nameof(MutualFundController), nameof(GetMutualFundSchemesAsync));
            return StatusCode(500, new { Error = e.Message });
        }
    }
}