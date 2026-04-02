#  Feature: High-level description of the Mutual Fund API functionality.
Feature: Mutual Fund Schemes API

    #  Business Value: Explains who the user is and why this feature exists.
    As an API consumer
    I want to retrieve mutual fund schemes
    So I can analyze investment options

#  Happy Path: Tests the standard successful retrieval of data.
Scenario: Retrieve mutual fund schemes

    #  Action: Simulates the GET request to the specific API route.
    When Endpoint "/mutualfund/schemes?pageNumber=1&searchText=others" is called

    #  Outcome: Verifies the standard HTTP 200 OK status.
    Then response status should be "200"

    #  Assertion: Ensures the body contains the expected fund data.
    And response should contains mutual fund schemes

#  Negative Path: Tests the system's resilience and error handling.
Scenario: Handle database outage gracefully

    #  Setup: Uses a "Given" step to simulate a backend infrastructure failure.
    Given the database connection should fails

    #  Action: Attempts the API call under failure conditions.
    When Endpoint "/mutualfund/schemes?pageNumber=1" is called

    #  Outcome: Verifies the API returns HTTP 500 Internal Server Error.
    Then response status should be "500"