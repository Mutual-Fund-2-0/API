using API.DTOs;
using API.Models;

namespace API.Mappers;

/// <summary>
/// Extension methods for mapping MutualFundScheme entities to pagination DTOs.
/// </summary>
public static class MutualFundSchemeMapper
{

    /// <summary>
    /// Converts raw entity list + pagination metadata into standardized PagedResultDTO.
    /// </summary>
    /// <param name="schemes">Current page's MutualFundScheme entities</param>
    /// <param name="pageNumber">Current page number</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="totalCount">Total records available for pagination calculation</param>
    /// <returns>Fully populated PagedResultDTO for API serialization</returns>
    public static PagedResultDTO<MutualFundScheme> ToPagedResultDTO(this List<MutualFundScheme> schemes, int pageNumber,int pageSize, int totalCount)
    {
        return new PagedResultDTO<MutualFundScheme>()
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = schemes
        };
    }
}
