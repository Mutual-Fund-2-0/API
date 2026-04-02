using System.Text.Json;
using API.DTOs;
using API.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Reqnroll;

namespace IT.Steps;

/// <summary>
/// Contains the step definitions for testing Mutual Fund API functionality using Reqnroll.
/// </summary>
[Binding]
public class MutualFundSteps
{

    /// <summary>
    /// Stores the HTTP response received from the API during the execution of a test scenario.
    /// </summary>
    private HttpResponseMessage _response = null!;

    /// <summary>
    /// Stores the deserialized data transfer object containing the paged result of mutual fund schemes.
    /// </summary>
    private PagedResultDTO _result = null!;

    /// <summary>
    /// Performs a GET request to the specified API endpoint and deserializes the result if successful.
    /// </summary>
    /// <param name="endpoint">The relative URL path of the API endpoint to be called.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [When(@"Endpoint ""(.*)"" is called")]
    public async Task WhenEndpointCalled(string endpoint)
    {
        _response = await TestHooks.Client.GetAsync(endpoint);
        var json = await _response.Content.ReadAsStringAsync();
        if(_response.IsSuccessStatusCode) _result = JsonSerializer.Deserialize<PagedResultDTO>(json)!;
    }

    /// <summary>
    /// Asserts that the HTTP response status code matches the expected status code.
    /// </summary>
    /// <param name="statusCode">The expected integer HTTP status code (e.g., 200, 500).</param>
    [Then(@"response status should be ""(.*)""")]
    public void ThenResponseStatusIs(int statusCode) => Assert.That((int)_response.StatusCode, Is.EqualTo(statusCode));

    /// <summary>
    /// Validates that the API response body contains a valid, non-null paged result of mutual fund schemes.
    /// </summary>
    [Then(@"response should contains mutual fund schemes")]
    public void ThenResponseShouldContainsMutualFundSchemes()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_result, Is.Not.Null);
            Assert.That(_result, Is.InstanceOf<PagedResultDTO>());
        });
    }

    /// <summary>
    /// Mocks the database repository to simulate a connection failure by throwing an exception.
    /// This uses WebApplicationFactory to override the real repository with a Moq object.
    /// </summary>
    [Given(@"the database connection should fails")]
    public static void WhenDatabaseConnectionFails()
    {
        var factory = TestHooks._factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var mockRepo = new Mock<IMutualFundRepository>();
                mockRepo.Setup(x => x.GetMutualFundSchemesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ThrowsAsync(new Exception("Database connection failed"));
                services.AddScoped(_ => mockRepo.Object);
            });
        });
        TestHooks.Client = factory.CreateClient();
    }
}
