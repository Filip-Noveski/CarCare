using CarCare.Persistence.Configuration;
using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CarCare.Persistence.Services;

internal class MigrationsHistoryRepository : IMigrationsHistoryRepository
{
    private const string TableName = "__MigrationHistory";
    private readonly DBContext _context;

    public MigrationsHistoryRepository(DBContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Migration migration)
    {
        using SqliteConnection connection = await _context.CreateConnection();

        string sql = $"""
            INSERT INTO {TableName} (Id, ProductVersion)
            VALUES (@Id, @ProductVersion)
            """;

        await connection.ExecuteAsync(sql, migration);
    }

    public async Task<bool> ExistsAsync()
    {
        using SqliteConnection connection = await _context.CreateConnection();

        string sql = $"""
            SELECT COUNT(*)
            FROM sqlite_master
            WHERE type = 'table' AND name = @TableName
            """;

        int count = connection.ExecuteScalar<int>(sql, new { TableName });

        return count > 0;
    }

    public async Task<Migration> GetLatestMigrationAsync()
    {
        using SqliteConnection connection = await _context.CreateConnection();

        string sql = $"""
            SELECT *
            FROM {TableName}
            ORDER BY Id DESC
            LIMIT 1
            """;

        Migration migration = await connection.QuerySingleAsync<Migration>(sql);

        return migration;
    }
}
