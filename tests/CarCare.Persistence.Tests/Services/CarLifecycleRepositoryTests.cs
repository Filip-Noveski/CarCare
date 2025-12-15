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
public class CarLifecycleRepositoryTests : RepositoryTestsGroup
{
    protected override string RelevantTablesSql => """
        CREATE TABLE IF NOT EXISTS Cars (
            Id TEXT PRIMARY KEY
        );
        
        CREATE TABLE IF NOT EXISTS [CarLifecycles] (
            [CarId] TEXT PRIMARY KEY,
            [PurchaseDate] TEXT NOT NULL,
            [PurchasePrice] NUMERIC NOT NULL,
            [PurchaseCurrency] TEXT NOT NULL,
            [SellDate] TEXT,
            [SellPrice] NUMERIC,
            [SellCurrency] TEXT,

            FOREIGN KEY ([CarId]) REFERENCES [Cars]([Id])
        );
        """;

    private readonly DBContext _context;
    private readonly CarLifecycleRepository _sut;

    public CarLifecycleRepositoryTests(ITestOutputHelper output) : base(output)
    {
        _context = new(Configuration);
        _sut = new(_context);
    }

    private async Task<CarLifecycleDao[]> AddStarterEntries()
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string addCarsSql = """
            INSERT INTO Cars (Id)
            VALUES (@Id)
            """;
        Guid id0 = Guid.NewGuid();
        Guid id1 = Guid.NewGuid();
        Guid id2 = Guid.NewGuid();
        await connection.ExecuteAsync(addCarsSql, new { Id = id0 });
        await connection.ExecuteAsync(addCarsSql, new { Id = id1 });
        await connection.ExecuteAsync(addCarsSql, new { Id = id2 });

        string addCarLifecycleSql = """
            INSERT INTO CarLifecycles (
                CarId, 
                PurchaseDate,
                PurchasePrice, 
                PurchaseCurrency, 
                SellDate,
                SellPrice,
                SellCurrency
            )
            VALUES (
                @CarId,
                @PurchaseDate,
                @PurchasePrice,
                @PurchaseCurrency,
                @SellDate,
                @SellPrice,
                @SellCurrency
            );
            """;
        CarLifecycleDao carLife0 = new(id0, new(2024, 12, 11), 3500, "EUR");
        CarLifecycleDao carLife1 = new(id1, new(2022, 6, 15), 1800, "EUR", new(2024, 12, 15), 1500, "EUR");
        CarLifecycleDao carLife2 = new(id2, new(2018, 3, 12), 62_000, "MKD", new(2020, 8, 23), 500, "EUR");
        await connection.ExecuteAsync(addCarLifecycleSql, carLife0);
        await connection.ExecuteAsync(addCarLifecycleSql, carLife1);
        await connection.ExecuteAsync(addCarLifecycleSql, carLife2);

        return new[] { carLife0, carLife1, carLife2 };
    }

    [Fact]
    public async Task ShouldAddNewWithNullSellData()
    {
        // Arrange
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        CarLifecycleDao life = new(Guid.NewGuid(), new(2023, 5, 20), 2500, "USD");
        string addCarSql = """
            INSERT INTO Cars (Id)
            VALUES (@Id)
            """;
        await connection.ExecuteAsync(addCarSql, new { Id = life.CarId });

        // Act
        await _sut.AddAsync(life);

        // Assert
        string sql = """
            SELECT *
            FROM CarLifecycles
            """;
        IEnumerable<CarLifecycleDao> result = await connection.QueryAsync<CarLifecycleDao>(sql);

        result.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(life);
    }

    [Fact]
    public async Task ShouldAddNewWithFilledSellData()
    {
        // Arrange
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        CarLifecycleDao life = new(Guid.NewGuid(), new(2023, 5, 20), 2500, "USD", 
            new(2025, 12, 1), 2000, "CAD");
        string addCarSql = """
            INSERT INTO Cars (Id)
            VALUES (@Id)
            """;
        await connection.ExecuteAsync(addCarSql, new { Id = life.CarId });

        // Act
        await _sut.AddAsync(life);

        // Assert
        string sql = """
            SELECT *
            FROM CarLifecycles
            """;
        IEnumerable<CarLifecycleDao> result = await connection.QueryAsync<CarLifecycleDao>(sql);

        result.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(life);
    }

    [Fact]
    public async Task ShouldDeleteById()
    {
        // Arrange
        CarLifecycleDao[] entries = await AddStarterEntries();

        // Act
        await _sut.DeleteAsync(entries[1].CarId);

        // Assert
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        string sql = """
            SELECT *
            FROM CarLifecycles
            """;
        IEnumerable<CarLifecycleDao> result = await connection.QueryAsync<CarLifecycleDao>(sql);

        result.Should().HaveCount(2)
            .And.ContainEquivalentOf(entries[0])
            .And.ContainEquivalentOf(entries[2]);
    }

    [Fact]
    public async Task ShouldGetById()
    {
        // Arrange
        CarLifecycleDao[] entries = await AddStarterEntries();

        // Act
        CarLifecycleDao result = await _sut.GetAsync(entries[2].CarId);

        // Assert
        result.Should().BeEquivalentTo(entries[2]);
    }

    [Fact]
    public async Task ShouldUpdateEntry()
    {
        // Arrange
        CarLifecycleDao[] entries = await AddStarterEntries();
        CarLifecycleDao newEntry = new(
            entries[1].CarId, new(2023, 1, 1), 2000, "USD", new(2024, 1, 1), 1800, "USD");

        // Act
        await _sut.UpdateAsync(newEntry);

        // Assert
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        string sql = """
            SELECT *
            FROM CarLifecycles
            """;
        IEnumerable<CarLifecycleDao> result = await connection.QueryAsync<CarLifecycleDao>(sql);

        result.Should().HaveCount(3)
            .And.ContainEquivalentOf(entries[0])
            .And.ContainEquivalentOf(newEntry)
            .And.ContainEquivalentOf(entries[2]);
    }
}
