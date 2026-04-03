using API.DTOs;
using API.Interfaces;
using API.Mappers;
using API.Models;

namespace API.Services;

/// <summary>
/// Business logic for mutual fund operations.
/// </summary>
/// <param name="logger">Logger instance for service layer</param>
/// <param name="repository">Repository for mutual fund scheme data access</param>
public class MutualFundService(ILogger<MutualFundService> logger, IMutualFundRepository repository) : IMutualFundService
{
    /// <summary>
    /// Retrieves mutual fund schemes from repository with search and pagination logic.
    /// </summary>
    /// <param name="pageNumber">Page number.</param>
    /// <param name="pageSize">Number of schemes per page.</param>
    /// <param name="searchText">Optional search filter.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>Mutual fund schemes.</returns>
    public async Task<PagedResultDTO<Scheme>> GetMutualFundSchemesAsync(int pageNumber, int pageSize, string? searchText, CancellationToken cancellationToken = default)
    {
        var (totalCount, schemes) = await repository.GetMutualFundSchemesAsync(pageNumber, pageSize, searchText, cancellationToken);
        if (schemes.Count == 0) logger.LogWarning("No mutual fund schemes found for search: {Search}", searchText);
        PagedResultDTO<Scheme> page = schemes.ToPagedResultDTO(pageNumber, pageSize, totalCount);
        return page;
    }
}
