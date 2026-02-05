using API.DTOs;
using API.Interfaces;
using API.Models;
using API.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Npgsql;

namespace UT.ServiceTests;

/// <summary>
/// Unit tests for MutualFundService business logic and error handling.
/// </summary>
[TestFixture]
public sealed class MutualFundServiceTests
{

    /// <summary>
    /// Mocked logger instance for dependency injection.
    /// </summary>
    private Mock<ILogger<MutualFundService>> _mockedLogger;

    /// <summary>
    /// Mocked MutualFundRepository for isolating controller logic.
    /// </summary>
    private Mock<IMutualFundRepository> _mockedRepository;

    /// <summary>
    /// Instance of MutualFundService. 
    /// </summary>
    private MutualFundService _service;

    /// <summary>
    /// Initializes mocks and creates service instance before each test.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _mockedLogger = new();
        _mockedRepository = new();

        _service = new(_mockedLogger.Object, _mockedRepository.Object);
    }

    /// <summary>
    /// Cleans up resources after each test.
    /// </summary>
    [OneTimeTearDown]
    public void Dispose() => (_service as IDisposable)?.Dispose();

    /// <summary>
    /// Verifies that GetMutualFundSchemesAsync returns a non‑null PagedResultDTO<MutualFundScheme> when the repository returns valid data.    
    /// </summary>
    /// <returns>Awaitable task for async test completion.</returns>
    [Test]
    public async Task GetMutualFundSchemesAsync_ReturnsData_WhenRepositoryReturnsData()
    {

        // Arrange
        _mockedRepository.Setup(repository => repository.GetMutualFundSchemesAsync(It.IsAny<int>())).ReturnsAsync((0, []));

        // Act
        var response = await _service.GetMutualFundSchemesAsync(It.IsAny<int>());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<PagedResultDTO<MutualFundScheme>>());
        });

        _mockedRepository.Verify(service => service.GetMutualFundSchemesAsync(It.IsAny<int>()), Times.Once);
    }

    /// <summary>
    /// Verifies that GetMutualFundSchemesAsync throws DbException when the repository throws DbException.
    /// </summary>
    /// <returns>Awaitable task for async test completion.</returns>
    [Test]
    public async Task GetMutualFundSchemesAsync_ThrowsException_WhenRepositoryThrowsException()
    {

        // Arrange
        _mockedRepository.Setup(repository => repository.GetMutualFundSchemesAsync(It.IsAny<int>())).ThrowsAsync(new NpgsqlException());

        // Act
        // Assert
        Assert.ThrowsAsync<NpgsqlException>(async () => await _service.GetMutualFundSchemesAsync(It.IsAny<int>()));

        _mockedRepository.Verify(repository => repository.GetMutualFundSchemesAsync(It.IsAny<int>()), Times.Once);
    }
}