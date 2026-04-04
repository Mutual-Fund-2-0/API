#  Feature: High-level description of the Mutual Fund API functionality.
Feature: Mutual Fund Schemes API
    As an API consumer
    I want to retrieve mutual fund schemes
    So I can analyze investment options

#  Happy Path: Tests the standard successful retrieval of data.
Scenario: Retrieve mutual fund schemes
    When Endpoint "/mutualfund/schemes?pageNumber=1&pageSize=10&searchText=others" is called
    Then response status should be "200"
    And response should contains mutual fund schemes

#  Negative Path: Tests the system's resilience and error handling.
Scenario: Handle database outage gracefully
    Given the database connection should fails
    When Endpoint "/mutualfund/schemes?pageNumber=1&pageSize=10" is called
    Then response status should be "500"
