using API.Models;

namespace API.Interfaces;

/// <summary>
/// Data access interface for mutual fund scheme repository operations.
/// </summary>
public interface IMutualFundRepository
{

    /// <summary>
    /// Retrieves the mutual fund schemes.
    /// </summary>
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="searchText">Optional text to filter schemes by name or category.</param>
    /// <returns>A tuple containing the total filtered count and the list of schemes.</returns>
    Task<(int, List<MutualFundScheme>)> GetMutualFundSchemesAsync(int pageNumber, string? searchText);
}
