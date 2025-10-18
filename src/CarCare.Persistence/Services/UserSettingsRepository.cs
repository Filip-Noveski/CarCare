using CarCare.Persistence.Configuration;
using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CarCare.Persistence.Services;

internal class UserSettingsRepository : IUserSettingsRepository
{
    private const string TableName = "UserSettings";
    private readonly DBContext _context;

    public UserSettingsRepository(DBContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UserSettingsDao settings)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        string sql = $"""
            INSERT INTO {TableName} (UserId, Theme)
            VALUES (@UserId, @Theme)
            """;

        await connection.ExecuteAsync(sql, settings);
    }

    public async Task DeleteAsync(Guid userId)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        string sql = $"""
            DELETE FROM {TableName}
            WHERE UserId = @UserId
            """;

        await connection.ExecuteAsync(sql, new { UserId = userId });
    }

    public async Task<UserSettingsDao> GetAsync(Guid userId)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        string sql = $"""
            SELECT * FROM {TableName}
            WHERE UserId = @UserId
            """;

        UserSettingsDao settings = await connection
            .QuerySingleAsync<UserSettingsDao>(sql, new { UserId = userId});
        return settings;
    }

    public async Task UpdateAsync(UserSettingsDao settings)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        string sql = $"""
            UPDATE {TableName}
            SET
                Theme = @Theme
            WHERE UserId = @UserId
            """;

        await connection.ExecuteAsync(sql, settings);
    }
}
