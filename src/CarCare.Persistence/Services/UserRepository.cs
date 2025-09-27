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

    public async Task AddAsync(User user)
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

    public async Task<User> GetUserAsync(Guid id)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            SELECT FROM {TableName}
            WHERE Id = @Id
            LIMIT 1
            """;

        User user = await connection.QuerySingleAsync(sql, new { Id = id });
        return user;
    }

    public async Task<User> GetUserAsync(string username)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            SELECT * FROM {TableName}
            WHERE Username = @Username
            LIMIT 1
            """;

        User user = await connection.QuerySingleAsync(sql, new { Username = username });
        return user;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();

        string sql = $"""
            SELECT * FROM {TableName}
            """;

        IEnumerable<User> users = await connection.QueryAsync<User>(sql);
        return users;
    }

    public async Task UpdateAsync(User user)
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
