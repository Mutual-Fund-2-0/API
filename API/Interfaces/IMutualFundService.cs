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
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="searchText">The optional text to filter schemes.</param>
    /// <returns>A paginated result containing mutual fund schemes.</returns>
    Task<PagedResultDTO> GetMutualFundSchemesAsync(int pageNumber, string? searchText);
}
