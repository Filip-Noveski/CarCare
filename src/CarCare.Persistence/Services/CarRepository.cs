using CarCare.Persistence.Configuration;
using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CarCare.Persistence.Services;

internal class CarRepository : ICarRepository
{
    private const string TableName = "Cars";
    private readonly DBContext _context;

    public CarRepository(DBContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CarDao car)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            INSERT INTO {TableName} (Id, UserId, Manufacturer, Model, Specification, ModelYear, Image)
            VALUES (@Id, @UserId, @Manufacturer, @Model, @Specification, @ModelYear, @Image);
            """;

        await connection.ExecuteAsync(sql, car);
    }

    public async Task DeleteAsync(Guid id)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            DELETE FROM {TableName}
            WHERE Id = @Id
            """;

        await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<IEnumerable<CarDao>> GetAllByUserAsync(Guid userId, int page = 0, int count = 10)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            SELECT * FROM {TableName}
            WHERE UserId = @UserId
            LIMIT @Count OFFSET @Skip
            """;

        IEnumerable<CarDao> result = await connection.QueryAsync<CarDao>(sql, new
        {
            UserId = userId,
            Count = count,
            Skip = (page - 1) * count
        });
        return result;
    }

    public async Task<CarDao> GetAsync(Guid id)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            SELECT * FROM {TableName}
            WHERE Id = @Id
            """;

        CarDao result = await connection.QuerySingleAsync<CarDao>(sql, new { Id = id });
        return result;
    }

    public async Task UpdateAsync(CarDao car)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            UPDATE {TableName}
            SET
                Manufacturer = @Manufacturer,
                Model = @Model,
                Specification = @Specification,
                ModelYear = @ModelYear,
                Image = @Image
            WHERE Id = @Id
            """;

        await connection.ExecuteAsync(sql, car);
    }
}
