using API.Interfaces;
using API.Models;

namespace API.Services;

/// <summary>
/// Business logic for mutual fund operations.
/// </summary>
/// <param name="logger">Logger instance for service layer</param>
/// <param name="repository">Repository for mutual fund scheme data access</param>
public class MutualFundService(ILogger<MutualFundService> logger, IMutualFundRepository repository) : IMutualFundService
{

    /// <summary>
    /// Logger instance for service.
    /// </summary>
    private readonly ILogger<MutualFundService> _logger = logger;

    /// <summary>
    /// Handles all database interactions through interface.
    /// </summary>
    private readonly IMutualFundRepository _repository = repository;

    /// <summary>
    /// EventId for service method entry points.
    /// </summary>
    private static readonly EventId MethodEntry = new(2000, "ServiceMethodEntry");

    /// <summary>
    /// EventId for successful service method completion.
    /// </summary>
    private static readonly EventId MethodExit = new(2001, "ServiceMethodExit");

    /// <summary>
    /// EventId for repository layer failures in service context.
    /// </summary>
    private static readonly EventId RepoError = new(2002, "RepositoryError");

    /// <summary>
    /// Retrieves total count of mutual fund schemes from repository.
    /// </summary>
    /// <returns>Total number of mutual fund schemes</returns>
    /// <exception cref="Exception">Rethrows repository exceptions with service context</exception>
    public async Task<List<MutualFundScheme>> GetMutualFundSchemesCountAsync()
    {
        _logger.LogDebug(MethodEntry, "Starting: {Service}-{Method}", nameof(MutualFundService), nameof(GetMutualFundSchemesCountAsync));
        try
        {
            var count = await _repository.GetMutualFundSchemesCountAsync();
            // if (count == 0)
                // _logger.LogWarning("No mutual fund schemes found");
            _logger.LogInformation("Retrieved {Count} mutual fund schemes", count);
            _logger.LogDebug(MethodExit, "{Service}-{Method}: Completed", nameof(MutualFundService), nameof(GetMutualFundSchemesCountAsync));
            return count;
        }
        catch(Exception e)
        {
            _logger.LogError(RepoError, e, "Repository failed in {Service}-{Method}", nameof(MutualFundService), nameof(GetMutualFundSchemesCountAsync));
            throw;
        }
    }
}