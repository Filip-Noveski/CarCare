using CarCare.Persistence.Configuration;
using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using CarCare.Persistence.Tests.Base;
using Dapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using NSubstitute;
using Xunit.Abstractions;

namespace CarCare.Persistence.Tests.Configuration;

public class MigrationsManagerTests : DBTestsGroup
{
    private readonly DBContext _context;
    private readonly IMigrationsHistoryRepository _historyRepository;
    private readonly MigrationsManager _sut;

    public MigrationsManagerTests(ITestOutputHelper output) : base(output)
    {
        _context = new(Configuration);
        _historyRepository = Substitute.For<IMigrationsHistoryRepository>();
        _sut = new(Configuration, _context, _historyRepository);
    }

    [Fact]
    public async Task ShouldCreateMigrationsTableAndRegisterFirstMigration()
    {
        // Arrange
        Migration migration = new(20250925);
        _historyRepository.AddAsync(migration).Returns(Task.CompletedTask);

        // Act
        await _sut.CreateMigrationHistoryTableAsync();

        // Assert
        // ensure table was created
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        string sql = $"""
            SELECT COUNT(*)
            FROM sqlite_master
            WHERE type = 'table' AND name = '__MigrationHistory'
            """;
        int count = await connection.ExecuteScalarAsync<int>(sql);
        count.Should().Be(1);

        // ensure record addition was requested
        await _historyRepository.Received(1).AddAsync(Arg.Is<Migration>(x => x.Id == migration.Id));
    }

    [Fact]
    public async Task ShouldUpdateToLatest()
    {
        // Arrange
        Migration latest = new(49950101);
        Migration mocked = new(50000101);
        Migration mockedToExclude = new(45950101);
        _historyRepository.GetLatestMigrationAsync().Returns(latest);
        _historyRepository.AddAsync(mocked).Returns(Task.CompletedTask);

        // Act
        await _sut.UpdateToLatestAsync();

        // Assert
        // ensure mock table was created
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        string sqlTest = $"""
            SELECT COUNT(*)
            FROM sqlite_master
            WHERE type = 'table' AND name = '__TestTable'
            """;
        int countTest = await connection.ExecuteScalarAsync<int>(sqlTest);
        countTest.Should().Be(1);

        // ensure "included" table was not created
        string sqlTestAlt = $"""
            SELECT COUNT(*)
            FROM sqlite_master
            WHERE type = 'table' AND name = '__TestTableAlt'
            """;
        int countTestAlt = await connection.ExecuteScalarAsync<int>(sqlTestAlt);
        countTestAlt.Should().Be(0);

        // ensure records were read and written
        await _historyRepository.Received(1).GetLatestMigrationAsync();
        await _historyRepository.Received(1).AddAsync(Arg.Is<Migration>(x => x.Id == mocked.Id));
        await _historyRepository.DidNotReceive().AddAsync(Arg.Is<Migration>(x => x.Id == mockedToExclude.Id));
    }
}
