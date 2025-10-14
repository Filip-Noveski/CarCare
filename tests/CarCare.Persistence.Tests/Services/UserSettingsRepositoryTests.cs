using CarCare.Persistence.Configuration;
using CarCare.Persistence.Models;
using CarCare.Persistence.Services;
using CarCare.Persistence.Tests.Base;
using Dapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Xunit.Abstractions;

namespace CarCare.Persistence.Tests.Services;

[Collection("RepositoryTests")]
public class UserSettingsRepositoryTests : RepositoryTestsGroup
{
    protected override string RelevantTablesSql => """
        CREATE TABLE IF NOT EXISTS Users (
            Id TEXT PRIMARY KEY
        );

        CREATE TABLE IF NOT EXISTS UserSettings (
            UserId TEXT PRIMARY KEY,
            Theme TEXT,

            FOREIGN KEY (UserId) REFERENCES Users(Id)
        );
        """;

    private readonly DBContext _context;
    private readonly UserSettingsRepository _sut;

    public UserSettingsRepositoryTests(ITestOutputHelper output) : base(output)
    {
        _context = new(Configuration);
        _sut = new(_context);
    }

    private async Task<UserSettingsDao[]> AddStarterEntries()
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string addUsersSql = """
            INSERT INTO Users (Id)
            VALUES (@Id)
            """;
        Guid id0 = Guid.NewGuid();
        Guid id1 = Guid.NewGuid();
        Guid id2 = Guid.NewGuid();
        await connection.ExecuteAsync(addUsersSql, new { Id = id0 });
        await connection.ExecuteAsync(addUsersSql, new { Id = id1 });
        await connection.ExecuteAsync(addUsersSql, new { Id = id2 });

        string addSettingsSql = """
            INSERT INTO UserSettings (UserId, Theme)
            VALUES (@UserId, @Theme)
            """;
        UserSettingsDao settings1 = new(id0, "Dark");
        UserSettingsDao settings2 = new(id1, "Light");
        UserSettingsDao settings3 = new(id2, "Blue");
        await connection.ExecuteAsync(addSettingsSql, settings1);
        await connection.ExecuteAsync(addSettingsSql, settings2);
        await connection.ExecuteAsync(addSettingsSql, settings3);

        return new[] { settings1, settings2, settings3 };
    }

    [Fact]
    public async Task ShouldAddSingleEntry()
    {
        // Arrange
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        string addUserSql = "INSERT INTO Users (Id) VALUES (@Id)";
        Guid id = Guid.NewGuid();
        await connection.ExecuteAsync(addUserSql, new { Id = id });
        UserSettingsDao settings = new(id, "Grey");

        // Act
        await _sut.AddAsync(settings);

        // Assert
        string sql = "SELECT * FROM UserSettings";
        IEnumerable<UserSettingsDao> result = await connection.QueryAsync<UserSettingsDao>(sql);

        result.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(settings);
    }

    [Fact]
    public async Task ShouldAddEntryAmongOthers()
    {
        // Arrange
        UserSettingsDao[] starterEntries = await AddStarterEntries();
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        string addUserSql = "INSERT INTO Users (Id) VALUES (@Id)";
        Guid id = Guid.NewGuid();
        await connection.ExecuteAsync(addUserSql, new { Id = id });
        UserSettingsDao input = new(id, "Grey");

        // Act
        await _sut.AddAsync(input);

        // Assert
        string sql = "SELECT * FROM UserSettings";
        IEnumerable<UserSettingsDao> result = await connection.QueryAsync<UserSettingsDao>(sql);

        result.Should().HaveCount(4)
            .And.BeEquivalentTo(
                new[] { starterEntries[0], starterEntries[1], starterEntries[2], input },
                options => options.WithStrictOrdering());
    }

    [Fact]
    public async Task ShouldDeleteEntry()
    {
        // Arrange
        UserSettingsDao[] starterEntries = await AddStarterEntries();

        // Act
        await _sut.DeleteAsync(starterEntries[1].UserId);

        // Assert
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        string sql = "SELECT * FROM UserSettings";
        IEnumerable<UserSettingsDao> result = await connection.QueryAsync<UserSettingsDao>(sql);

        result.Should().HaveCount(2)
            .And.BeEquivalentTo(
                new[] { starterEntries[0], starterEntries[2] },
                options => options.WithStrictOrdering());
    }

    [Fact]
    public async Task ShouldGetAppropriateSettings()
    {
        // Arrange
        UserSettingsDao[] starterEntries = await AddStarterEntries();

        // Act
        UserSettingsDao result = await _sut.GetAsync(starterEntries[1].UserId);

        // Assert
        result.Should().BeEquivalentTo(starterEntries[1]);
    }

    [Fact]
    public async Task ShouldUpdateSettings()
    {
        // Arrange
        UserSettingsDao[] starterEntries = await AddStarterEntries();
        UserSettingsDao newSettings1 = new(starterEntries[1].UserId, "Grey");

        // Act
        await _sut.UpdateAsync(newSettings1);

        // Assert
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        string sql = "SELECT * FROM UserSettings";
        IEnumerable<UserSettingsDao> result = await connection.QueryAsync<UserSettingsDao>(sql);

        result.Should().HaveCount(3)
            .And.BeEquivalentTo(
                new[] { starterEntries[0], newSettings1, starterEntries[2] },
                options => options.WithStrictOrdering());
    }
}
