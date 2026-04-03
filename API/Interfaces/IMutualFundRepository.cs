using API.Models;

namespace API.Interfaces;

/// <summary>
/// Data access interface for mutual fund scheme repository operations.
/// </summary>
public interface IMutualFundRepository
{
    /// <summary>
    /// Retrieves a filtered and paginated list of mutual fund schemes.
    /// </summary>
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="pageSize">Number of schemes per page.</param>
    /// <param name="searchText">Optional text to filter schemes by name or category.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A tuple containing the total filtered count and the readonly collection of schemes.</returns>
    Task<(int totalCount, IReadOnlyCollection<Scheme> schemes)> GetMutualFundSchemesAsync(int pageNumber, int pageSize, string? searchText, CancellationToken cancellationToken = default);
}
