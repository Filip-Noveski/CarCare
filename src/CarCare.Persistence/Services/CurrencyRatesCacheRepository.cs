using CarCare.Persistence.Configuration;
using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CarCare.Persistence.Services;

internal class CurrencyRatesCacheRepository : ICurrencyRatesCacheRepository
{
    private const string TableName = "CurrencyRatesCache";
    private const int MaxCacheEntries = 5;
    private readonly DBContext _context;

    public CurrencyRatesCacheRepository(DBContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CurrencyRateDao currencyRateDao)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            -- insert new entry
            INSERT INTO {TableName} (FromCurrency, ToCurrency, Rate, Date)
            VALUES (@FromCurrency, @ToCurrency, @Rate, @Date);

            -- evict oldest if cache limit is exceeded
            DELETE FROM {TableName}
            WHERE Id NOT IN (
                SELECT Id
                FROM {TableName}
                ORDER BY CachedAt DESC
                LIMIT {MaxCacheEntries}
            );
            """;

        await connection.ExecuteAsync(sql, currencyRateDao);
    }

    public async Task<CurrencyRateDao?> GetIfExistsAsync(string fromCurrency, string toCurrency, DateOnly date)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            SELECT * FROM {TableName}
            WHERE 
                FromCurrency = @FromCurrency 
                AND ToCurrency = @ToCurrency 
                AND Date = @Date
            LIMIT 1
            """;

        CurrencyRateDao? rate = await connection.QuerySingleOrDefaultAsync<CurrencyRateDao>(sql, new 
        { 
            FromCurrency = fromCurrency,
            ToCurrency = toCurrency,
            Date = date
        });
        return rate;
    }
}
