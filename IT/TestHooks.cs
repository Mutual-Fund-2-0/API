using Reqnroll;
using Reqnroll.BoDi;

namespace IT;

/// <summary>
/// Provides shared HTTP client and WebApplicationFactory setup for API integration tests.
/// Manages lifecycle of test clients across Reqnroll scenarios using Before/After hooks.
/// </summary>
[Binding]
public class TestHooks
{
    /// <summary>
    /// Custom WebApplicationFactory for hosting the API under test with test-specific configuration.
    /// </summary>
    public static CustomWebApplicationFactory Factory = null!;

    /// <summary>
    /// Initializes test infrastructure before each scenario runs.
    /// </summary>

    [BeforeTestRun]
    public static void Setup()
    {
        Factory = new CustomWebApplicationFactory();
    }

    /// <summary>
    /// Creates a fresh, isolated HttpClient for each scenario and registers it.
    /// </summary>
    [BeforeScenario]
    public static void SetupScenario(IObjectContainer objectContainer)
    {
        var client = Factory.CreateClient();
        objectContainer.RegisterInstanceAs(client);
    }

    /// <summary>
    /// Cleans up test resources after each scenario completes.
    /// </summary>
    [AfterTestRun]
    public static void Teardown()
    {
        Factory?.Dispose();
    }
}
