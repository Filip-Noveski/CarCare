using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Services;
using CarCare.Persistence.Tests.Base;
using NSubstitute;
using Xunit.Abstractions;

namespace CarCare.Persistence.Tests.Services;

public class DatabaseManagerTests : DBTestsGroup
{
    private readonly IMigrationsHistoryRepository _historyRepository;
    private readonly IMigrationsManager _migrationsManager;
    private readonly DatabaseManager _sut;

    public DatabaseManagerTests(ITestOutputHelper output) : base(output)
    {
        _historyRepository = Substitute.For<IMigrationsHistoryRepository>();
        _migrationsManager = Substitute.For<IMigrationsManager>();
        _sut = new(_historyRepository, _migrationsManager);
    }

    [Fact]
    public async Task ShouldRequestDatabaseUpdateIfExists()
    {
        // Arrange
        _historyRepository.ExistsAsync().Returns(true);
        _migrationsManager.UpdateToLatestAsync().Returns(Task.CompletedTask);

        // Act
        await _sut.VerifyDatabaseStateAsync();

        // Assert
        await _historyRepository.Received(1).ExistsAsync();
        await _migrationsManager.Received(1).UpdateToLatestAsync();
        await _migrationsManager.DidNotReceive().CreateMigrationHistoryTableAsync();
    }

    [Fact]
    public async Task ShouldRequestDatabaseCreationAndUpdateIfNotExists()
    {
        // Arrange
        _historyRepository.ExistsAsync().Returns(false);
        _migrationsManager.CreateMigrationHistoryTableAsync().Returns(Task.CompletedTask);
        _migrationsManager.UpdateToLatestAsync().Returns(Task.CompletedTask);

        // Act
        await _sut.VerifyDatabaseStateAsync();

        // Assert
        await _historyRepository.Received(1).ExistsAsync();
        await _migrationsManager.Received(1).CreateMigrationHistoryTableAsync();
        await _migrationsManager.Received(1).UpdateToLatestAsync();
    }
}
