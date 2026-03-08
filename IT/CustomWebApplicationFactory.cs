using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using API.Datas;

namespace IT;

/// <summary>
/// Custom WebApplicationFactory for integration testing the Mutual Fund Schemes API.
/// Overrides default service configuration to use in-memory database and test environment.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MFDbContext>));
            if (descriptor != null) services.Remove(descriptor);
            services.AddDbContext<MFDbContext>(options => options.UseInMemoryDatabase("ITDB"));
        });
    }
}
