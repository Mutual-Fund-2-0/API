using System.Text.Json;
using API.DTOs;
using API.Interfaces;
using API.Mappers;

namespace API.Services;

/// <summary>
/// Business logic for mutual fund operations.
/// </summary>
/// <param name="logger">Logger instance for service layer</param>
/// <param name="repository">Repository for mutual fund scheme data access</param>
public class MutualFundService(ILogger<MutualFundService> logger, IMutualFundRepository repository) : IMutualFundService
{
    /// <summary>
    /// Logger instance for service.
    /// </summary>
    private readonly ILogger<MutualFundService> _logger = logger;

    /// <summary>
    /// Handles all database interactions through repository.
    /// </summary>
    private readonly IMutualFundRepository _repository = repository;

    /// <summary>
    /// Retrieves mutual fund schemes from repository with search and pagination logic.
    /// </summary>
    /// <param name="pageNumber">Page number.</param>
    /// <param name="pageSize">Number of schemes per page.</param>
    /// <param name="searchText">Optional search filter.</param>
    /// <returns>Mutual fund schemes.</returns>
    /// <exception cref="Exception">Rethrows repository exceptions with service context</exception>
    public async Task<PagedResultDTO> GetMutualFundSchemesAsync(int pageNumber, int pageSize, string? searchText)
    {
        _logger.LogInformation("Starting: {Service}-{Method} with Search: {Search}", nameof(MutualFundService), nameof(GetMutualFundSchemesAsync), searchText);
        try
        {
            var (totalCount, schemes) = await _repository.GetMutualFundSchemesAsync(pageNumber, pageSize, searchText);
            if (schemes.Count == 0) _logger.LogWarning("No mutual fund schemes found for search: {Search}", searchText);
            _logger.LogDebug("Retrieved {Count} mutual fund schemes - {Schemes}", totalCount, JsonSerializer.Serialize(schemes));
            PagedResultDTO page = schemes.ToPagedResultDTO(pageNumber, pageSize, totalCount);
            _logger.LogInformation("Converted page result from schemes {Page}", page);
            return page;
        }
        catch(Exception e)
        {
            _logger.LogError(e, "Repository failed in {Service} - {Method}", nameof(MutualFundService), nameof(GetMutualFundSchemesAsync));
            throw;
        }
    }
}