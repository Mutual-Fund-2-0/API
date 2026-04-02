using API.DTOs;

namespace API.Interfaces;

/// <summary>
/// Defines service layer contracts for mutual fund operations.
/// </summary>
public interface IMutualFundService
{
    /// <summary>
    /// Retrieves a paginated list of mutual fund schemes based on search criteria.
    /// </summary>
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="pageSize">The number of schemes per page.</param>
    /// <param name="searchText">The optional text to filter schemes.</param>
    /// <returns>A paginated result containing mutual fund schemes.</returns>
    Task<PagedResultDTO> GetMutualFundSchemesAsync(int pageNumber, int pageSize, string? searchText);
}
