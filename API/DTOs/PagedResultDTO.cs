namespace API.DTOs;

/// <summary>
/// A generic pagination response DTO for any data model.
/// </summary>
/// <typeparam name="T">The type of the paginated items.</typeparam>
public record PagedResultDTO<T>
{
    /// <summary>
    /// Mutual Fund Scheme list.
    /// </summary>
    public required IReadOnlyCollection<T> Items { get; init; }

    /// <summary>
    /// Total records available across all pages.
    /// </summary>
    public required int TotalCount { get; init; }

    /// <summary>
    /// Current page number.
    /// </summary>
    public required int PageNumber { get; init; }

    /// <summary>
    /// Total pages available.
    /// </summary>
    public required int TotalPages { get; init; }

    /// <summary>
    /// Enables/disables Next button in client pagination controls.
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>
    /// Enables/disables Previous button in client pagination controls.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;
}
