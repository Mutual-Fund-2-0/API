using API.Controllers;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UT.Utils;

namespace UT.ControllerTests;

/// <summary>
/// Unit tests for MutualFundController endpoint behaviors and error handling.
/// </summary>
[TestFixture]
public sealed class MutualFundControllerTests
{
    /// <summary>
    /// Mocked MutualFundService for isolating controller logic.
    /// </summary>
    private Mock<IMutualFundService> _mockedService;

    /// <summary>
    /// Instance of MutualFundController. 
    /// </summary>
    private MutualFundController _controller;

    /// <summary>
    /// Initializes mocks and creates controller instance before each test.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _mockedService = new();

        _controller = new(_mockedService.Object);
    }

    /// <summary>
    /// Cleans up resources.
    /// </summary>
    [OneTimeTearDown]
    public void Dispose() => (_controller as IDisposable)?.Dispose();

    /// <summary>
    /// Verifies GetMutualFundSchemesAsync returns 200 OK with valid PagedResultDTO when service succeeds.
    /// </summary>
    /// <returns>Awaitable task for async test completion.</returns>
    [Test]
    public async Task GetMutualFundSchemesAsync_ReturnsOk_WhenServiceReturnsData()
    {

        // Arrange
        _mockedService.Setup(service => service.GetMutualFundSchemesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(TestSeedData.GetPagedResultDTO());

        // Act
        var response = await _controller.GetMutualFundSchemesAsync(1, 1, "test") as OkObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(response.Value, Is.Not.Null);
            Assert.That(response.Value, Is.InstanceOf<PagedResultDTO<Scheme>>());
        });

        _mockedService.Verify(service => service.GetMutualFundSchemesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
    }
}
