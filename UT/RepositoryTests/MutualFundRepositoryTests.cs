using API.Datas;
using API.Models;
using API.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace UT.RepositoryTests;

/// <summary>
/// Unit tests for MutualFundService business logic and error handling.
/// </summary>
[TestFixture]
public sealed class MutualFundRepositoriesTests
{
    
    /// <summary>
    /// Seed data for mutual fund list.
    /// </summary>
    private readonly List<MutualFundScheme> schemes = [
        new() {
            FundHouse = "TestFunds",
            SchemeType = "TestScheme"
        }
    ];

    /// <summary>
    /// Mocked logger instance for dependency injection.
    /// </summary>
    private Mock<ILogger<MutualFundRepository>> _mockedLogger;

    /// <summary>
    /// Instance of MutualFundService. 
    /// </summary>
    public required MutualFundRepository _repository;

    /// <summary>
    /// Initializes mocks and creates service instance before each test.
    /// </summary>
    [OneTimeSetUp]
    public async Task SetupAsync() => _mockedLogger = new();

    /// <summary>
    /// Cleans up resources after each test.
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
        var options = new DbContextOptionsBuilder<MFDbContext>().UseInMemoryDatabase(databaseName: "TestDB").Options;
        await using var context = new MFDbContext(options);
        context.MutualFundSchemes.AddRange(this.schemes);
        await context.SaveChangesAsync();
        _repository = new(_mockedLogger.Object, context);

        // Act
        var (totalCount, schemes) = await _repository.GetMutualFundSchemesAsync(It.IsAny<int>());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(totalCount, Is.Not.EqualTo(0));
            Assert.That(schemes, Is.InstanceOf<List<MutualFundScheme>>());
        });
    }
}