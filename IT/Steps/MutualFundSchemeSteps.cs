using System.Text.Json;
using API.DTOs;
using API.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Reqnroll;

namespace IT.Steps;

[Binding]
public class MutualFundSteps
{
    private HttpResponseMessage _response = null!;

    private PagedResultDTO _result = null!;

    [When(@"Endpoint ""(.*)"" is called")]
    public async Task WhenEndpointCalled(string endpoint)
    {
        _response = await TestHooks.Client.GetAsync(endpoint);
        var json = await _response.Content.ReadAsStringAsync();
        if(_response.IsSuccessStatusCode) _result = JsonSerializer.Deserialize<PagedResultDTO>(json)!;
    }

    [Then(@"response status should be ""(.*)""")]
    public void ThenResponseStatusIs(int statusCode) => Assert.That((int)_response.StatusCode, Is.EqualTo(statusCode));

    [Then(@"response should contains mutual fund schemes")]
    public void ThenResponseShouldContainsMutualFundSchemes()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_result, Is.Not.Null);
            Assert.That(_result, Is.InstanceOf<PagedResultDTO>());
        });
    }

    [Given(@"the database connection should fails")]
    public static void WhenDatabaseConnectionFails()
    {
        var factory = TestHooks._factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var mockRepo = new Mock<IMutualFundRepository>();
                mockRepo.Setup(x => x.GetMutualFundSchemesAsync(It.IsAny<int>())).ThrowsAsync(new Exception("Database connection failed"));
                services.AddScoped(_ => mockRepo.Object);
            });
        });
        TestHooks.Client = factory.CreateClient();
    }
}
