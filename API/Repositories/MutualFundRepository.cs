using System.Text.Json;
using API.Constants;
using API.Datas;
using API.Interfaces;
using API.Models;

using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

/// <summary>
/// Handles all database interactions for mutual fund scheme data.
/// </summary>
/// <param name="logger">Logger instance for repository.</param>
/// <param name="context">EF Core context for database operations.</param>
public class MutualFundRepository(ILogger<MutualFundRepository> logger, MFDbContext context) : IMutualFundRepository
{
    /// <summary>
    /// Logger instance for repository operation.
    /// </summary>
    private readonly ILogger<MutualFundRepository> _logger = logger;

    /// <summary>
    /// Database context for access.
    /// </summary>
    private readonly MFDbContext _context = context;

    /// <summary>
    /// Returns the schemes in MutualFundSchemes in the database.
    /// </summary>
    /// <param name="pageNumber">Page number.</param>
    /// <param name="searchText">Optional text to filter schemes by name or category.</param>
    /// <returns>Mutual fund schemes.</returns>
    /// <exception cref="Exception">Thrown when database operation fails.</exception>
    public async Task<(int, List<MutualFundScheme>)> GetMutualFundSchemesAsync(int pageNumber, string? searchText)
    {
        _logger.LogInformation("Starting: {Repository} - {Method} | Search: {Search}", nameof(MutualFundRepository), nameof(GetMutualFundSchemesAsync), searchText);
        try
        {
            var query = _context.MutualFundSchemes.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                string loweredSearchText = searchText.ToLower();
                query = query.Where(scheme => scheme.Name!.ToLower().Contains(loweredSearchText) || scheme.Category!.ToLower().Contains(loweredSearchText) || scheme.House!.ToLower().Contains(loweredSearchText) || scheme.Plan!.ToLower().Contains(loweredSearchText) || scheme.SubCategory!.ToLower().Contains(loweredSearchText) || scheme.Type!.ToLower().Contains(loweredSearchText));
            }
            int totalCount = await query.CountAsync();
            int offset = (pageNumber - 1) * PageDefaults.PageSize;
            var schemes = await query.OrderBy(scheme => scheme.Code).Skip(offset).Take(PageDefaults.PageSize).ToListAsync();
            _logger.LogDebug("{Repository} - {Method}: Completed. Found {Count} schemes {Schemes}", nameof(MutualFundRepository), nameof(GetMutualFundSchemesAsync), totalCount, JsonSerializer.Serialize(schemes));
            return (totalCount, schemes);
        }
        catch(Exception e)
        {
            _logger.LogError(e, "{Repository} - {Method}: Failed", nameof(MutualFundRepository), nameof(GetMutualFundSchemesAsync));
            throw;
        }
    }
}