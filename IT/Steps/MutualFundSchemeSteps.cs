using System.Text.Json;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Reqnroll;
using Moq;
using Microsoft.Extensions.DependencyInjection;

namespace IT.Steps;

[Binding]
public class MutualFundSteps
{
    private HttpResponseMessage _response = null!;

    private PagedResultDTO<MutualFundScheme> _result = null!;

    [When(@"Endpoint ""(.*)"" is called")]
    public async Task WhenEndpointCalled(string endpoint)
    {
        _response = await TestHooks.Client.GetAsync(endpoint);
        var json = await _response.Content.ReadAsStringAsync();
        _result = JsonSerializer.Deserialize<PagedResultDTO<MutualFundScheme>>(json)!;
    }

    [Given(@"the database connection should fails")]
    public static void WhenDatabaseConnectionFails()
    {
        var factory = TestHooks._factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IMutualFundRepository));
                if (descriptor != null) services.Remove(descriptor);
                var mockRepo = new Mock<IMutualFundRepository>();
                mockRepo.Setup(x => x.GetMutualFundSchemesAsync(It.IsAny<int>())).ThrowsAsync(new Exception("Database connection failed"));
                services.AddScoped(_ => mockRepo.Object);
            });
        });

        TestHooks.Client = factory.CreateClient();
    }

    [Then(@"response status should be ""(.*)""")]
    public void ThenResponseStatusIs(int statusCode) => Assert.That((int)_response.StatusCode, Is.EqualTo(statusCode));

    [Then(@"response should contains (.*) mutual fund schemes")]
    public void ThenResponseShouldContainsMutualFundSchemes()
    {
        Assert.That(_result, Is.Not.Null);
    }

}
