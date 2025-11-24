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
public class CarRepositoryTests : RepositoryTestsGroup
{
    protected override string RelevantTablesSql => """
        CREATE TABLE IF NOT EXISTS Users (
            Id TEXT PRIMARY KEY
        );
        
        CREATE TABLE IF NOT EXISTS [Cars] (
            [Id] TEXT PRIMARY KEY,
            [UserId] TEXT,
            [Manufacturer] TEXT NOT NULL,
            [Model] TEXT NOT NULL,
            [Specification] TEXT NOT NULL,
            [ModelYear] INTEGER NOT NULL,
            [Image] BLOB,

            FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
        );
        """;

    private readonly DBContext _context;
    private readonly CarRepository _sut;

    public CarRepositoryTests(ITestOutputHelper output) : base(output)
    {
        _context = new(Configuration);
        _sut = new(_context);
    }

    private async Task<CarDao[]> AddStarterEntries()
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string addUsersSql = """
            INSERT INTO Users (Id)
            VALUES (@Id)
            """;
        Guid id0 = Guid.NewGuid();
        Guid id1 = Guid.NewGuid();
        await connection.ExecuteAsync(addUsersSql, new { Id = id0 });
        await connection.ExecuteAsync(addUsersSql, new { Id = id1 });

        string addCarSql = """
            INSERT INTO Cars (Id, UserId, Manufacturer, Model, Specification, ModelYear, Image)
            VALUES (@Id, @UserId, @Manufacturer, @Model, @Specification, @ModelYear, @Image);
            """;
        CarDao car0 = new(Guid.NewGuid(), id0, "Renault", "Clio", "1.2 16V", 2008);
        CarDao car1 = new(Guid.NewGuid(), id1, "Ford", "Fiesta", "ST", 2021, new byte[] { 0x40, 0x80 });
        CarDao car2 = new(Guid.NewGuid(), id1, "Lotus", "Elise", "Cup 250", 2019);
        await connection.ExecuteAsync(addCarSql, car0);
        await connection.ExecuteAsync(addCarSql, car1);
        await connection.ExecuteAsync(addCarSql, car2);

        return new[] { car0, car1, car2 };
    }

    [Fact]
    public async Task ShouldAddNewCarWithoutImage()
    {
        // Arrange
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        string addUserSql = "INSERT INTO Users (Id) VALUES (@Id)";
        Guid id = Guid.NewGuid();
        await connection.ExecuteAsync(addUserSql, new { Id = id });
        CarDao car = new(Guid.NewGuid(), id, "Hyundai", "i20", "N", 2023);

        // Act
        await _sut.AddAsync(car);

        // Assert
        string sql = "SELECT * FROM Cars";
        IEnumerable<CarDao> result = await connection.QueryAsync<CarDao>(sql);

        result.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(car);
    }

    [Fact]
    public async Task ShouldAddNewCarWithImage()
    {
        // Arrange
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        string addUserSql = "INSERT INTO Users (Id) VALUES (@Id)";
        Guid id = Guid.NewGuid();
        await connection.ExecuteAsync(addUserSql, new { Id = id });
        CarDao car = new(Guid.NewGuid(), id, "Hyundai", "i20", "N", 2023, new byte[] { 0x21, 0x32, 0x48 });

        // Act
        await _sut.AddAsync(car);

        // Assert
        string sql = "SELECT * FROM Cars";
        IEnumerable<CarDao> result = await connection.QueryAsync<CarDao>(sql);

        result.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(car);
    }

    [Fact]
    public async Task ShouldDeleteById()
    {
        // Arrange
        CarDao[] entries = await AddStarterEntries();
        Guid id = entries[1].Id;

        // Act
        await _sut.DeleteAsync(id);

        // Assert
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        string sql = "SELECT * FROM Cars";
        IEnumerable<CarDao> result = await connection.QueryAsync<CarDao>(sql);

        result.Should().HaveCount(2);
        result.ElementAt(0).Should().BeEquivalentTo(entries[0]);
        result.ElementAt(1).Should().BeEquivalentTo(entries[2]);
    }

    [Fact]
    public async Task ShouldGetAllEntriesByUserId()
    {
        // Arrange
        CarDao[] entries = await AddStarterEntries();
        Guid id = entries[1].UserId;

        // Act
        IEnumerable<CarDao> result = await _sut.GetAllByUserAsync(id);

        // Assert
        result.Should().HaveCount(2);
        result.ElementAt(0).Should().BeEquivalentTo(entries[1]);
        result.ElementAt(1).Should().BeEquivalentTo(entries[2]);
    }

    [Fact]
    public async Task ShouldGetEntryById()
    {
        // Arrange
        CarDao[] entries = await AddStarterEntries();
        Guid id = entries[2].Id;

        // Act
        CarDao result = await _sut.GetAsync(id);

        // Assert
        result.Should().BeEquivalentTo(entries[2]);
    }

    [Fact]
    public async Task ShouldUpdateEntry()
    {
        // Arrange
        CarDao[] entries = await AddStarterEntries();
        CarDao newEntry = new(
            entries[2].Id, entries[2].UserId, "Mini", "Cooper", "JCW", 2018, new byte[] { 0x90, 0x50 });

        // Act
        await _sut.UpdateAsync(newEntry);

        // Assert
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        string sql = "SELECT * FROM Cars";
        IEnumerable<CarDao> result = await connection.QueryAsync<CarDao>(sql);

        result.Should().HaveCount(3);
        result.ElementAt(0).Should().BeEquivalentTo(entries[0]);
        result.ElementAt(1).Should().BeEquivalentTo(entries[1]);
        result.ElementAt(2).Should().BeEquivalentTo(newEntry);
    }
}
