using CarCare.Persistence.Models;
using CarCare.Persistence.Services;
using CarCare.Persistence.Tests.Base;
using Dapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Xunit.Abstractions;

namespace CarCare.Persistence.Tests.Services;

public class MigrationsHistoryRepositoryTests : RepositoryTestsGroup
{
    protected override string RelevantTablesSql => """
        CREATE TABLE IF NOT EXISTS __MigrationHistory (
            Id INTEGER PRIMARY KEY
        );
        """;

    private readonly MigrationsHistoryRepository _sut;

    public MigrationsHistoryRepositoryTests(ITestOutputHelper output) : base(output)
    {
        _sut = new(Context);
    }

    [Fact]
    public async Task ShouldAddEntry()
    {
        // Arrange
        MigrationDao migration = new(12340205);

        // Act
        await _sut.AddAsync(migration);

        // Assert
        using SqliteConnection connection = await Context.CreateConnectionAsync();
        string sql = """
            SELECT * FROM __MigrationHistory
            """;
        IEnumerable<MigrationDao> migrations = await connection.QueryAsync<MigrationDao>(sql);

        migrations.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(migration);
    }

    [Fact]
    public async Task ShouldFindTable()
    {
        // Arrange: do nothing

        // Act
        bool exists = await _sut.ExistsAsync();

        // Assert
        exists.Should().Be(true);
    }

    [Fact]
    public async Task ShouldNotFindTable()
    {
        // Arrange
        await DisposeAsync();   // delete databse

        // Act
        bool exists = await _sut.ExistsAsync();

        // Assert
        exists.Should().Be(false);
    }

    [Fact]
    public async Task ShouldNotFindEntry()
    {
        // Arrange
        Func<Task<MigrationDao>> func = _sut.GetLatestMigrationAsync;

        // Act & Assert
        await func.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task ShouldGetLastEntry()
    {
        // Arrange
        using SqliteConnection connection = await Context.CreateConnectionAsync();
        string addSql = """
            INSERT INTO __MigrationHistory (ID)
            VALUES (20250105), (20250205), (20250921)
            """;
        await connection.ExecuteAsync(addSql);

        // Act
        MigrationDao migration = await _sut.GetLatestMigrationAsync();

        // Assert
        migration.Should().BeEquivalentTo(new MigrationDao(20250921));
    }
}
