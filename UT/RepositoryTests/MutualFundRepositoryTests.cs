using API.Datas;
using API.Models;
using API.Repositories;
using Microsoft.EntityFrameworkCore;
using UT.Utils;

namespace UT.RepositoryTests;

/// <summary>
/// Unit tests for MutualFundService data logic and error handling.
/// </summary>
[TestFixture]
public sealed class MutualFundRepositoriesTests
{
    /// <summary>
    /// Instance of MutualFundRepository. 
    /// </summary>
    public required MutualFundRepository _repository;

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
        context.Schemes.AddRange(TestSeedData.GetMutualFundSchemes());
        await context.SaveChangesAsync();
        _repository = new(context);

        // Act
        var (totalCount, _schemes) = await _repository.GetMutualFundSchemesAsync(1, 1, "test");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(totalCount, Is.Not.EqualTo(0));
            Assert.That(_schemes, Is.InstanceOf<List<Scheme>>());
        });
    }
}
