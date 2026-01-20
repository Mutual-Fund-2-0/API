using API.DTOs;
using API.Models;

namespace API.Mappers;

public static class MutualFundSchemeMapper
{
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
