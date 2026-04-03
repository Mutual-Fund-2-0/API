using API.Datas;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

/// <summary>
/// Handles all database interactions for mutual fund scheme data.
/// </summary>
/// <param name="context">EF Core context for database operations.</param>
public class MutualFundRepository(MFDbContext context) : IMutualFundRepository
{
    /// <summary>
    /// Returns the schemes in MutualFundSchemes in the database.
    /// </summary>
    /// <param name="pageNumber">Page number.</param>
    /// <param name="pageSize">Number of schemes per page.</param>
    /// <param name="searchText">Optional text to filter schemes by name or category.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>Mutual fund schemes.</returns>
    public async Task<(int totalCount, IReadOnlyCollection<Scheme> schemes)> GetMutualFundSchemesAsync(int pageNumber, int pageSize, string? searchText, CancellationToken cancellationToken = default)
    {
        var query = context.Schemes.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            string loweredSearchText = searchText.ToLower();
            query = query.Where(scheme => scheme.Name!.ToLower().Contains(loweredSearchText) || scheme.Category!.ToLower().Contains(loweredSearchText) || scheme.House!.ToLower().Contains(loweredSearchText) || scheme.Plan!.ToLower().Contains(loweredSearchText) || scheme.SubCategory!.ToLower().Contains(loweredSearchText) || scheme.Type!.ToLower().Contains(loweredSearchText));
        }
        int totalCount = await query.CountAsync(cancellationToken);
        int offset = (pageNumber - 1) * pageSize;
        var schemes = await query.OrderBy(scheme => scheme.Code).Skip(offset).Take(pageSize).ToListAsync(cancellationToken);
        return (totalCount, schemes);
    }
}
