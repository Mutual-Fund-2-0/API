namespace API.Interfaces;

/// <summary>
/// Data access interface for mutual fund scheme repository operations.
/// </summary>
public interface IMutualFundRepository
{
    /// <summary>
    /// Asynchronously retrieves the total count of mutual fund schemes from database.
    /// </summary>
    /// <returns>Total number of mutual fund schemes in database</returns>
    /// <exception cref="Exception">Thrown for database failures</exception>
    Task<int> GetMutualFundSchemesCountAsync();
}
