using API.DTOs;

namespace API.Interfaces;

/// <summary>
/// Defines service layer contracts.
/// </summary>
public interface IMutualFundService
{

    /// <summary>
    /// Retrieves mutual fund schemes.
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <returns>Mutual fund schemes</returns>
    /// <exception cref="Exception">Rethrows repository exceptions with service context</exception>
    Task<PagedResultDTO> GetMutualFundSchemesAsync(int pageNumber);
}
