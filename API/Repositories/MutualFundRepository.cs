using System.Diagnostics;
using API.Datas;
using API.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

/// <summary>
/// Handles all database interactions for mutual fund scheme data.
/// </summary>
/// <param name="logger">Logger instance for repository</param>
/// <param name="context">EF Core context for database operations</param>
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
    /// EventId for logging method entry points.
    /// </summary>
    private static readonly EventId MethodEntry = new(1000, "MethodEntry");

    /// <summary>
    /// EventId for logging successful method completion.
    /// </summary>
    private static readonly EventId MethodExit = new(1001, "MethodExit");

    /// <summary>
    /// EventId for database-related exceptions.
    /// </summary>
    private static readonly EventId DbError = new(1002, "DatabaseError");

    /// <summary>
    /// EventId for slow database queries.
    /// </summary>
    private static readonly EventId SlowQuery = new(1003, "SlowQuery");

    /// <summary>
    /// Returns the total count of MutualFundSchemes in the database.
    /// </summary>
    /// <returns>Total number of mutual fund schemes</returns>
    /// <exception cref="Exception">Thrown when database operation fails</exception>
    public async Task<int> GetMutualFundSchemesCountAsync()
    {
        _logger.LogDebug(MethodEntry, "Starting: {Repository}-{Method}", nameof(MutualFundRepository), nameof(GetMutualFundSchemesCountAsync));
        try
        {
            var stopwatch = Stopwatch.StartNew();
            var count = await _context.MutualFundSchemes.CountAsync();
            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds > 500)
                _logger.LogWarning(SlowQuery, "Query took {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);
            _logger.LogDebug(MethodExit, "{Repository}-{Method}: Completed, count={Count}", nameof(MutualFundRepository), nameof(GetMutualFundSchemesCountAsync), count);
            return count;
        }
        catch(Exception e)
        {
            _logger.LogError(DbError, e, "{Repository}-{Method}: Failed", nameof(MutualFundRepository), nameof(GetMutualFundSchemesCountAsync));
            throw;
        }
    }
}