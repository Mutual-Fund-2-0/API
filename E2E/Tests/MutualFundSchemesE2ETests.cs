using System.Text.Json;
using API.DTOs;
using API.Models;
using Microsoft.Playwright;

namespace E2E.Tests;

/// <summary>
/// 
/// </summary>
[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class MutualFundSchemesE2ETests : PlaywrightTest
{

    private IAPIRequestContext _request;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _request = await Playwright.APIRequest.NewContextAsync(new()
        {
            BaseURL = "http://localhost:5291"
        });
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _request.DisposeAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task Should_Return_Paged_MutualFundSchemes_When_PageNumber_Is_1()
    {
        // Act
        var response = await _request.GetAsync("/MutualFund/schemes?pageNumber=1");
        var json = await response.JsonAsync();

        // Assert
        Assert.That(response.Status, Is.EqualTo(200));

        // Deserialize into our DTOs
        var page = await JsonSerializer.DeserializeAsync<PagedResultDTO<MutualFundScheme>>(json);
        
        new PagedResultDTO<MutualFundScheme>(
            Items: json.Value.GetProperty("items")
                       .EnumerateArray()
                       .Select(item => new MutualFundScheme(
                           FundHouse: item.GetProperty("fundHouse").GetString()!,
                           SchemeType: item.GetProperty("schemeType").GetString()!,
                           SchemeCategory: item.GetProperty("schemeCategory").GetString()!,
                           SchemeCode: item.GetProperty("schemeCode").GetInt32(),
                           SchemeName: item.GetProperty("schemeName").GetString()!,
                           IsinGrowth: item.TryGetProperty("isinGrowth", out var isinGrowth)
                               ? isinGrowth.GetString()
                               : null,
                           IsinDivReinvestment: item.TryGetProperty("isinDivReinvestment", out var isinDiv)
                               ? isinDiv.GetString()
                               : null
                       ))
                       .ToList(),
            TotalCount: json.Value.GetProperty("totalCount").GetInt32(),
            PageNumber: json.Value.GetProperty("pageNumber").GetInt32(),
            PageSize: json.Value.GetProperty("pageSize").GetInt32(),
            TotalPages: json.Value.GetProperty("totalPages").GetInt32(),
            HasNextPage: json.Value.GetProperty("hasNextPage").GetBoolean(),
            HasPreviousPage: json.Value.GetProperty("hasPreviousPage").GetBoolean()
        );

        // Business‑level assertions
        Assert.That(page.Items, Has.Count.GreaterThan(0));
        Assert.That(page.PageNumber, Is.EqualTo(1));
        Assert.That(page.PageSize, Is.EqualTo(10));
        Assert.That(page.HasPreviousPage, Is.False);
        Assert.That(page.HasNextPage, Is.True);
        Assert.That(page.TotalCount, Is.EqualTo(37330));

        // Pick one item and assert its shape
        var first = page.Items.First();
        Assert.That(first.SchemeCode, Is.GreaterThan(0));
        Assert.That(first.SchemeName, Does.Contain("Fund"));
    }
}
