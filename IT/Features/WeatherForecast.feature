Feature: Weather Forecast

    As a client of the API
    I want to fetch the weather forecast
    So that I can see upcoming weather details

    Scenario: Get weather forecast returns 5 items
        When the client calls the weather forecast API
        Then the response status should be 200
        And the response should contain 5 weather forecasts
