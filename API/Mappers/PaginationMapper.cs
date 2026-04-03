using API.DTOs;

namespace API.Mappers;

/// <summary>
/// Universal extension methods for mapping any data collection to pagination DTOs.
/// </summary>
public static class PaginationMapper
{
    /// <summary>
    /// Converts a raw read-only collection + pagination metadata into a standardized PagedResultDTO.
    /// </summary>
    /// <typeparam name="T">The type of the items being paginated.</typeparam>
    /// <param name="items">Current page's items</param>
    /// <param name="pageNumber">Current page number</param>
    /// <param name="pageSize">Number of items per page.</param>
    /// <param name="totalCount">Total records available for pagination calculation</param>
    /// <returns>Fully populated PagedResultDTO for API serialization</returns>
    public static PagedResultDTO<T> ToPagedResultDTO<T>(this IReadOnlyCollection<T> items, int pageNumber, int pageSize, int totalCount)
    {
        return new PagedResultDTO<T>
        {
            PageNumber = pageNumber,
            TotalCount = totalCount,
            TotalPages = totalCount == 0 ? 0 : (totalCount + pageSize - 1) / pageSize,
            Items = items
        };
    }
}
