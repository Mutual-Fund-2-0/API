using Reqnroll;
using System.Text.Json;
using API.Models;

namespace IT.Steps;

[Binding]
public class WeatherForecastSteps
{
    private HttpResponseMessage _response = null!;
    private List<WeatherForecast> _result = null!;

    [When(@"the client calls the weather forecast API")]
    public async Task WhenTheClientCallsTheWeatherForecastApi()
    {
        _response = await TestHooks.Client.GetAsync("/WeatherForecast");

        var json = await _response.Content.ReadAsStringAsync();
        _result = JsonSerializer.Deserialize<List<WeatherForecast>>(json)!;
    }

    [Then(@"the response status should be (.*)")]
    public void ThenTheResponseStatusShouldBe(int statusCode) => Assert.That((int)_response.StatusCode, Is.EqualTo(statusCode));

    [Then(@"the response should contain (.*) weather forecasts")]
    public void ThenTheResponseShouldContainWeatherForecasts(int expectedCount)
    {
        Assert.That(_result, Is.Not.Null);
        Assert.That(_result.Count, Is.EqualTo(expectedCount));
    }
}
