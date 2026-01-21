namespace API.DTOs;

/// <summary>
/// Generic pagination response DTO for REST APIs.
/// </summary>
/// <typeparam name="T">DTO type MutualFundScheme</typeparam>
public class PagedResultDTO<T>
{

    /// <summary>
    /// Current page's data items.
    /// </summary>
    public List<T> Items { get; set; } = [];

    /// <summary>
    /// Total records available across all pages.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Current page number.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total pages available.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    /// Enables/disables Next button in client pagination controls.
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>
    /// Enables/disables Previous button in client pagination controls.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;
}
