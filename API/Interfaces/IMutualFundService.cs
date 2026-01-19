namespace API.Interfaces;

/// <summary>
/// Defines service layer contracts.
/// </summary>
public interface IMutualFundService
{
    /// <summary>
    /// Asynchronously retrieves total count of mutual fund schemes.
    /// </summary>
    /// <returns>Total number of valid mutual fund schemes</returns>
    /// <exception cref="Exception">Rethrows repository exceptions with service context</exception>
    Task<int> GetMutualFundSchemesCountAsync();
}
