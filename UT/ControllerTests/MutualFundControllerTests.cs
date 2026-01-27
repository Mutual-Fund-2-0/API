using System.Text.Json;
using API.Controllers;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;

namespace UT.ControllerTests;

public sealed partial class MutualFundControllerTests
{
    private Mock<ILogger<MutualFundController>> _mockedLogger;
    private Mock<IHttpContextAccessor> _mockedAccessor;
    private Mock<IMutualFundService> _mockedService;
    private MutualFundController _controller;
    /// <summary>
    /// 
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _mockedLogger = new();
        _mockedAccessor = new();
        _mockedService = new();

        _mockedAccessor.Setup(accessor => accessor.HttpContext!.TraceIdentifier).Returns(string.Empty);

        _controller = new(_mockedLogger.Object, _mockedAccessor.Object, _mockedService.Object);
    }

    /// <summary>
    /// 
    /// </summary>
    [TearDown]
    public void Dispose()
    {}

    [Test]
    public async Task GetMutualFundSchemes_ReturnsOk_WhenServiceReturnsData()
    {
        _mockedService.Setup(service => service.GetMutualFundSchemesAsync(It.IsAny<int>())).ReturnsAsync(new PagedResultDTO<MutualFundScheme>());
        var response = await _controller.GetMutualFundSchemesAsync(It.IsAny<int>()) as OkObjectResult;
        Assert.Multiple(() =>
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(response.Value, Is.Not.Null);
            dynamic result = response.Value!;
            var correlationId = result.GetType().GetProperty("correlationId")?.GetValue(result, null)?.ToString();
            var page = result.GetType().GetProperty("page")?.GetValue(result, null);
            Assert.That(correlationId, Is.EqualTo(string.Empty));
            Assert.That(page, Is.InstanceOf<PagedResultDTO<MutualFundScheme>>());
        });
    }
}