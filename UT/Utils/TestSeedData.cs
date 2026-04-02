using API.DTOs;
using API.Models;

namespace UT.Utils;

/// <summary>
/// Centralized mock data for Unit Tests to prevent duplication.
/// </summary>
public static class TestSeedData
{
    /// <summary>
    /// Returns a standardized list of mutual fund schemes for DB and Repository testing.
    /// </summary>
    public static List<MutualFundScheme> GetMutualFundSchemes() =>
    [
        new()
        {
            Code = 0,
            House = "TestFunds",
        }
    ];

    /// <summary>
    /// Returns a standardized PagedResultDTO for Service and Controller testing.
    /// </summary>
    public static PagedResultDTO GetPagedResultDTO() => new()
    {
        Schemes = GetMutualFundSchemes(),
        TotalCount = 1
    };
}