using CarCare.Persistence.Configuration;
using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CarCare.Persistence.Services;

internal class UserRepository : IUserRepository
{
    private const string TableName = "Users";
    private readonly DBContext _context;

    public UserRepository(DBContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UserDao user)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            INSERT INTO {TableName} (Id, Username, Password, Avatar)
            VALUES (@Id, @Username, @Password, @Avatar);
            """;

        await connection.ExecuteAsync(sql, user);
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

    public async Task DeleteAsync(string username)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            DELETE FROM {TableName}
            WHERE Username = @Username
            """;

        await connection.ExecuteAsync(sql, new { Username = username });
    }

    public async Task<UserDao> GetUserAsync(Guid id)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            SELECT * FROM {TableName}
            WHERE Id = @Id
            LIMIT 1
            """;

        UserDao user = await connection.QuerySingleAsync<UserDao>(sql, new { Id = id });
        return user;
    }

    public async Task<UserDao> GetUserAsync(string username)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            SELECT * FROM {TableName}
            WHERE Username = @Username
            LIMIT 1
            """;

        UserDao user = await connection.QuerySingleAsync<UserDao>(sql, new { Username = username });
        return user;
    }

    public async Task<IEnumerable<UserDao>> GetUsersAsync()
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            SELECT * FROM {TableName}
            """;

        IEnumerable<UserDao> users = await connection.QueryAsync<UserDao>(sql);
        return users;
    }

    public async Task UpdateAsync(UserDao user)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            UPDATE {TableName}
            SET
                Username = @Username,
                Password = @Password,
                Avatar = @Avatar
            WHERE Id = @Id
            """;

        await connection.ExecuteAsync(sql, user);
    }
}
