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
    /// <param name="pageNumber">Current page number</param>
    /// <returns>Mutual fund schemes</returns>
    /// <exception cref="Exception">Thrown for database failures</exception>
    Task<(int, List<MutualFundScheme>)> GetMutualFundSchemesAsync(int pageNumber);
}
