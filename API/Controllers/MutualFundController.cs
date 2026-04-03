using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers;

/// <summary>
/// REST API controller for mutual fund scheme operations.
/// </summary>
/// <param name="service">Service for mutual fund operations</param>
[ApiController]
[Route("[controller]")]
[EnableRateLimiting("IpRateLimit")]
public class MutualFundController(IMutualFundService service) : ControllerBase
{
    /// <summary>
    /// GET endpoint returning mutual fund schemes with search and pagination.
    /// </summary>
    /// <param name="pageNumber">The current page index.</param>
    /// <param name="pageSize">Number of schemes per page.</param>
    /// <param name="searchText">Optional text to filter schemes by name or category.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A paginated list of mutual fund schemes</returns>
    [HttpGet("schemes", Name = "GetMutualFundSchemes")]
    [ProducesResponseType(typeof(PagedResultDTO<Scheme>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMutualFundSchemesAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? searchText = null, CancellationToken cancellationToken = default)
    {
        var page = await service.GetMutualFundSchemesAsync(pageNumber, pageSize, searchText, cancellationToken);
        return Ok(page);
    }
}
