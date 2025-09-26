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
        using SqliteConnection connection = await _context.CreateConnection();

        string sql = $"""
            SELECT COUNT(*)
            FROM sqlite_master
            WHERE type = 'table' AND name = '__MigrationHistory'
            """;

        int count = await connection.ExecuteScalarAsync<int>(sql);

        count.Should().Be(1);

        await _historyRepository.Received(1).AddAsync(Arg.Is<Migration>(x => x.Id == migration.Id));
    }
}
