using API.Datas;
using API.Models;
using API.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Npgsql;
using UT.Utils;

namespace UT.RepositoryTests;

/// <summary>
/// Unit tests for MutualFundService data logic and error handling.
/// </summary>
[TestFixture]
public sealed class MutualFundRepositoriesTests
{

    /// <summary>
    /// Mocked logger instance for dependency injection.
    /// </summary>
    private Mock<ILogger<MutualFundRepository>> _mockedLogger;

    /// <summary>
    /// Instance of MutualFundRepository. 
    /// </summary>
    public required MutualFundRepository _repository;

    /// <summary>
    /// Initializes mocks.
    /// </summary>
    [OneTimeSetUp]
    public async Task SetupAsync() => _mockedLogger = new();

    /// <summary>
    /// Cleans up resources.
    /// </summary>
    [OneTimeTearDown]
    public void Dispose() => (_repository as IDisposable)?.Dispose();

    /// <summary>
    /// Verifies that GetMutualFundSchemesAsync returns a non‑null PagedResultDTO<MutualFundScheme> when the repository returns valid data.
    /// </summary>
    /// <returns>Awaitable task for async test completion.</returns>
    [Test]
    public async Task GetMutualFundSchemesAsync_ReturnsData_WhenRepositoryReturnsData()
    {

        // Arrange
        var options = new DbContextOptionsBuilder<MFDbContext>().UseInMemoryDatabase(databaseName: "UnitTestDB").Options;
        await using var context = new MFDbContext(options);
        context.MutualFundSchemes.AddRange(TestSeedData.GetMutualFundSchemes());
        await context.SaveChangesAsync();
        _repository = new(_mockedLogger.Object, context);

        // Act
        var (totalCount, _schemes) = await _repository.GetMutualFundSchemesAsync(1, "test");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(totalCount, Is.Not.EqualTo(0));
            Assert.That(_schemes, Is.InstanceOf<List<MutualFundScheme>>());
        });
    }

    /// <summary>
    /// Verifies that GetMutualFundSchemesAsync returns an exception when the database is down.
    /// </summary>
    /// <returns>Awaitable task for async test completion.</returns>
    [Test]
    public async Task GetMutualFundSchemesAsync_ThrowsException_WhenNetworkFailure()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<MFDbContext>().UseInMemoryDatabase(databaseName: "FailureTestDB").Options;
        var mockContext = new Mock<MFDbContext>(options);

        mockContext.Setup(c => c.MutualFundSchemes).Throws(new NpgsqlException("Connection to server failed"));

        var repository = new MutualFundRepository(_mockedLogger.Object, mockContext.Object);

        // Act & Assert
        Assert.ThrowsAsync<NpgsqlException>(async () => await repository.GetMutualFundSchemesAsync(It.IsAny<int>(), It.IsAny<string>()));
    }
}