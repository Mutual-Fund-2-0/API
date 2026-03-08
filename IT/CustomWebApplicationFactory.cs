using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IT;

/// <summary>
/// Custom WebApplicationFactory for integration testing the Mutual Fund Schemes API.
/// Overrides default service configuration to use in-memory database and test environment.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{

    /// <summary>
    /// Configures the test web host with Testing environment and replaces production DbContext
    /// </summary>
    /// <param name="builder">The web host builder to configure for testing.</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.UseEnvironment("Testing");
}
