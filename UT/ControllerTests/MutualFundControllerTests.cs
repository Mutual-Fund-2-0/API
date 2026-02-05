using System.Data.Common;
using API.Controllers;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace UT.ControllerTests;

/// <summary>
/// Unit tests for MutualFundController endpoint behaviors and error handling.
/// </summary>
[TestFixture]
public sealed class MutualFundControllerTests
{

    /// <summary>
    /// Mocked logger instance for dependency injection.
    /// </summary>
    private Mock<ILogger<MutualFundController>> _mockedLogger;

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
        _mockedLogger = new();
        _mockedService = new();

        _controller = new(_mockedLogger.Object, _mockedService.Object);
    }

    /// <summary>
    /// Cleans up resources after each test.
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
        _mockedService.Setup(service => service.GetMutualFundSchemesAsync(It.IsAny<int>())).ReturnsAsync(new PagedResultDTO<MutualFundScheme>());

        // Act
        var response = await _controller.GetMutualFundSchemesAsync(It.IsAny<int>()) as OkObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(response.Value, Is.Not.Null);
            Assert.That(response.Value, Is.InstanceOf<PagedResultDTO<MutualFundScheme>>());
        });

        _mockedService.Verify(service => service.GetMutualFundSchemesAsync(It.IsAny<int>()), Times.Once);
    }

    /// <summary>
    /// Verifies GetMutualFundSchemesAsync returns 500 InternalServerError when service throws DbException.
    /// </summary>
    /// <returns>Awaitable task for async test completion.</returns>
    [Test]
    public async Task GetMutualFundSchemesAsync_ReturnsInternalServerError_WhenServiceThrowsException()
    {

        // Arrange
        _mockedService.Setup(service => service.GetMutualFundSchemesAsync(It.IsAny<int>())).ThrowsAsync(It.IsAny<DbException>());

        // Act
        var response = await _controller.GetMutualFundSchemesAsync(It.IsAny<int>()) as ObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response!.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        });

        _mockedService.Verify(service => service.GetMutualFundSchemesAsync(It.IsAny<int>()), Times.Once);
    }
}