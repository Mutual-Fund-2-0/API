using API.Constants;
using API.Models;

namespace API.DTOs;

/// <summary>
/// Pagination response DTO.
/// </summary>
public class PagedResultDTO
{

    /// <summary>
    /// Mutual Fund Scheme list.
    /// </summary>
    public List<MutualFundScheme> Schemes { get; set; } = [];

    /// <summary>
    /// Total records available across all pages.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Current page number.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Total pages available.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageDefaults.PageSize);

    /// <summary>
    /// Enables/disables Next button in client pagination controls.
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>
    /// Enables/disables Previous button in client pagination controls.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;
}
