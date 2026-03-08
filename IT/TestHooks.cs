using Reqnroll;

namespace IT;

/// <summary>
/// Provides shared HTTP client and WebApplicationFactory setup for API integration tests.
/// Manages lifecycle of test clients across Reqnroll scenarios using Before/After hooks.
/// </summary>
[Binding]
public class TestHooks
{

    /// <summary>
    /// Shared HTTP client for making API calls during integration tests.
    /// </summary>
    public static HttpClient Client = null!;

    /// <summary>
    /// Custom WebApplicationFactory for hosting the API under test with test-specific configuration.
    /// </summary>
    public static CustomWebApplicationFactory _factory = null!;

    /// <summary>
    /// Initializes test infrastructure before each scenario runs.
    /// </summary>

    [BeforeScenario]
    public static void Setup()
    {
        _factory = new CustomWebApplicationFactory();
        Client = _factory.CreateClient();
    }

    /// <summary>
    /// Cleans up test resources after each scenario completes.
    /// </summary>
    [AfterScenario]
    public static void Teardown()
    {
        Client?.Dispose();
        _factory?.Dispose();
    }
}
