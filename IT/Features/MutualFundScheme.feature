Feature: Mutual Fund Schemes API
    As an API consumer
    I want to retrieve mutual fund schemes
    So I can analyze investment options

Scenario: Retrieve mutual fund schemes
    When Endpoint "/mutualfund/schemes?pageNumber=1" is called
    Then response status should be "200"
    And response should contains mutual fund schemes

Scenario: Handle database outage gracefully
    Given the database connection should fails
    When I GET "/mutualfund/schemes?pageNumber=1"
    Then response status should be "500"
