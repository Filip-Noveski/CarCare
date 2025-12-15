using CarCare.Persistence.Configuration;
using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CarCare.Persistence.Services;

internal class CarLifecycleRepository : ICarLifecycleRepository
{
    private const string TableName = "CarLifecycles";
    private readonly DBContext _context;

    public CarLifecycleRepository(DBContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CarLifecycleDao carLifecycle)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            INSERT INTO {TableName} (
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

        await connection.ExecuteAsync(sql, carLifecycle);
    }

    public async Task DeleteAsync(Guid carId)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            DELETE FROM {TableName}
            WHERE CarId = @CarId
            """;

        await connection.ExecuteAsync(sql, new { CarId = carId });
    }

    public async Task<CarLifecycleDao> GetAsync(Guid carId)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            SELECT * FROM {TableName}
            WHERE CarId = @CarId
            """;

        CarLifecycleDao result = await connection.QuerySingleAsync<CarLifecycleDao>(sql, new { CarId = carId });
        return result;
    }

    public async Task UpdateAsync(CarLifecycleDao carLifecycle)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            UPDATE {TableName}
            SET 
                PurchaseDate = @PurchaseDate,
                PurchasePrice = @PurchasePrice,
                PurchaseCurrency = @PurchaseCurrency,
                SellDate = @SellDate,
                SellPrice = @SellPrice,
                SellCurrency = @SellCurrency
            WHERE CarId = @CarId
            """;

        await connection.ExecuteAsync(sql, carLifecycle);
    }
}
