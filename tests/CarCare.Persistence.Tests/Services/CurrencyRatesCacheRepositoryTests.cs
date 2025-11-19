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
public class CurrencyRatesCacheRepositoryTests : RepositoryTestsGroup
{
    protected override string RelevantTablesSql => """
        CREATE TABLE IF NOT EXISTS [CurrencyRatesCache] (
            [Id] INTEGER PRIMARY KEY,
            [FromCurrency] TEXT NOT NULL,
            [ToCurrency] TEXT NOT NULL,
            [Rate] NUMERIC NOT NULL,
            [Date] TEXT NOT NULL,
            [CachedAt] TEXT NOT NULL DEFAULT (DATETIME('now'))
        );
        """;

    private readonly DBContext _context;
    private readonly CurrencyRatesCacheRepository _sut;

    public CurrencyRatesCacheRepositoryTests(ITestOutputHelper output) : base(output)
    {
        _context = new(Configuration);
        _sut = new(_context);
    }

    private async Task AddBaseEntries()
    {
        using SqliteConnection connection = await Context.CreateConnectionAsync();
        string sql = $"""
            INSERT INTO CurrencyRatesCache (Id, FromCurrency, ToCurrency, Rate, Date, CachedAt)
            VALUES
                (1, 'AUD', 'EUR', 1.0, '2025-10-12', '2025-10-12 18:26:32'),
                (2, 'MKD', 'EUR', 3.0, '2025-11-18', '2025-10-19 18:28:32'),
                (3, 'EUR', 'AUD', 4.0, '2025-09-06', '2024-10-12 18:26:33'),
                (4, 'GBP', 'USD', 5.0, '2023-12-18', '2025-11-12 18:32:18'),
                (5, 'AUD', 'GBP', 6.0, '2024-02-12', '2025-09-11 18:26:32')
            """;
        await connection.ExecuteAsync(sql);
    }

    [Fact]
    public async Task ShouldAddSingleEntry()
    {
        // Arrange
        CurrencyRateDao rate = new("EUR", "AUD", 2.0, new(2025, 11, 18));

        // Act
        await _sut.AddAsync(rate);

        // Assert
        using SqliteConnection connection = await Context.CreateConnectionAsync();
        string sql = "SELECT * FROM CurrencyRatesCache";
        IEnumerable<CurrencyRateDao> result = await connection.QueryAsync<CurrencyRateDao>(sql);
        result.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(rate);
    }

    [Fact]
    public async Task ShouldAddNewAndEvictOldest()
    {
        // Arrange
        await AddBaseEntries();
        CurrencyRateDao rate = new("EUR", "AUD", 2.0, new(2025, 11, 18));

        // Act
        await _sut.AddAsync(rate);

        // Assert
        using SqliteConnection connection = await Context.CreateConnectionAsync();
        string sql = "SELECT * FROM CurrencyRatesCache";
        IEnumerable<CurrencyRateDao> result = await connection.QueryAsync<CurrencyRateDao>(sql);
        result.Should().HaveCount(5);
        result.ElementAt(0).Should().Match<CurrencyRateDao>(
            r => r.FromCurrency == "AUD"
            && r.ToCurrency == "EUR"
            && r.Rate == 1.0
            && r.Date == new DateOnly(2025, 10, 12));
        result.ElementAt(1).Should().Match<CurrencyRateDao>(
            r => r.FromCurrency == "MKD"
            && r.ToCurrency == "EUR"
            && r.Rate == 3.0
            && r.Date == new DateOnly(2025, 11, 18));
        result.ElementAt(2).Should().Match<CurrencyRateDao>(
            r => r.FromCurrency == "GBP"
            && r.ToCurrency == "USD"
            && r.Rate == 5.0
            && r.Date == new DateOnly(2023, 12, 18));
        result.ElementAt(3).Should().Match<CurrencyRateDao>(
            r => r.FromCurrency == "AUD"
            && r.ToCurrency == "GBP"
            && r.Rate == 6.0
            && r.Date == new DateOnly(2024, 02, 12));
        result.ElementAt(4).Should().BeEquivalentTo(rate);
    }

    [Fact]
    public async Task ShouldFindAndReturnRate()
    {
        // Arrange
        await AddBaseEntries();

        // Act
        CurrencyRateDao? result = await _sut.GetIfExistsAsync("GBP", "USD", new(2023, 12, 18));

        // Assert
        result.Should().NotBeNull()
            .And.Match<CurrencyRateDao>(
            r => r.FromCurrency == "GBP"
            && r.ToCurrency == "USD"
            && r.Rate == 5.0
            && r.Date == new DateOnly(2023, 12, 18));
    }

    [Fact]
    public async Task ShouldNotFindByFromCurrency()
    {
        // Arrange
        await AddBaseEntries();

        // Act
        CurrencyRateDao? result = await _sut.GetIfExistsAsync("RSD", "USD", new(2023, 12, 18));

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ShouldNotFindByToCurrency()
    {
        // Arrange
        await AddBaseEntries();

        // Act
        CurrencyRateDao? result = await _sut.GetIfExistsAsync("GBP", "RUB", new(2023, 12, 18));

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ShouldNotFindByDate()
    {
        // Arrange
        await AddBaseEntries();

        // Act
        CurrencyRateDao? result = await _sut.GetIfExistsAsync("GBP", "USD", new(2022, 12, 18));

        // Assert
        result.Should().BeNull();
    }
}
