using API.DTOs;
using API.Models;

namespace API.Interfaces;

/// <summary>
/// Defines service layer contracts.
/// </summary>
public interface IMutualFundService
{
    /// <summary>
    /// Retrieves mutual fund schemes.
    /// </summary>
    /// <param name="pageNumber">Current page number</param>
    /// <returns>Mutual fund schemes</returns>
    /// <exception cref="Exception">Rethrows repository exceptions with service context</exception>
    Task<PagedResultDTO<MutualFundScheme>> GetMutualFundSchemesAsync(int pageNumber);
}
