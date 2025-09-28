using CarCare.Persistence.Configuration;
using CarCare.Persistence.Models;
using CarCare.Persistence.Services;
using CarCare.Persistence.Tests.Base;
using Dapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Xunit.Abstractions;

namespace CarCare.Persistence.Tests.Services;

public class UserRepositoryTests : RepositoryTestsGroup
{
    protected override string RelevantTablesSql => """
        CREATE TABLE IF NOT EXISTS Users (
            Id TEXT PRIMARY KEY,
            Username TEXT NOT NULL UNIQUE,
            Password TEXT NOT NULL,
            Avatar BLOB
        );
        """;

    private readonly DBContext _context;
    private readonly UserRepository _sut;

    public UserRepositoryTests(ITestOutputHelper output) : base(output)
    {
        _context = new(Configuration);
        _sut = new(_context);
    }

    private async Task<List<UserDao>> AddBasicUsersAsync(byte[]? avatar = null)
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        UserDao user1 = new(Guid.NewGuid(), "User1", "Password1", avatar);
        UserDao user2 = new(Guid.NewGuid(), "User2", "Password2", avatar);
        UserDao user3 = new(Guid.NewGuid(), "User3", "Password3", avatar);

        string sqlInsert = """
            INSERT INTO Users (Id, Username, Password, Avatar)
            VALUES (@Id, @Username, @Password, @Avatar)
            """;
        await connection.ExecuteAsync(sqlInsert, user1);
        await connection.ExecuteAsync(sqlInsert, user2);
        await connection.ExecuteAsync(sqlInsert, user3);

        return new List<UserDao> { user1, user2, user3 };
    }

    private async Task<IEnumerable<UserDao>> GetAllUsersAsync()
    {
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        string sql = """
            SELECT * FROM Users
            """;
        IEnumerable<UserDao> result = await connection.QueryAsync<UserDao>(sql);
        return result;
    }

    [Fact]
    public async Task ShouldAddNewUserWithoutAvatarToEmpty()
    {
        // Arrange
        UserDao user = new(Guid.NewGuid(), "Some User", "Some Password");

        // Act
        await _sut.AddAsync(user);

        // Assert
        IEnumerable<UserDao> result = await GetAllUsersAsync();

        result.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task ShouldAddNewUserWithoutAvatarToNonEmpty()
    {
        // Arrange
        List<UserDao> users = await AddBasicUsersAsync();
        UserDao userIn = new(Guid.NewGuid(), "Some User", "Some Password");
        users.Add(userIn);

        // Act
        await _sut.AddAsync(userIn);

        // Assert
        IEnumerable<UserDao> result = await GetAllUsersAsync();

        result.Should().HaveCount(4)
            .And.BeEquivalentTo(users, options => options.WithStrictOrdering());
    }

    [Fact]
    public async Task ShouldRefuseNewUserWithoutAvatarWithSameName()
    {
        // Arrange
        List<UserDao> users = await AddBasicUsersAsync();
        UserDao userIn = new(Guid.NewGuid(), users[1].Username, "Some Password");

        Func<Task> f = async () => await _sut.AddAsync(userIn);

        // Act & Assert
        await f.Should().ThrowAsync<SqliteException>();
    }

    [Fact]
    public async Task ShouldRefuseNewUserWithoutAvatarWithSameId()
    {
        // Arrange
        List<UserDao> users = await AddBasicUsersAsync();
        UserDao userIn = new(users[2].Id, "User1", "Some Password");

        Func<Task> f = async () => await _sut.AddAsync(userIn);

        // Act & Assert
        await f.Should().ThrowAsync<SqliteException>();
    }

    [Fact]
    public async Task ShouldAddNewUserWithAvatarToEmpty()
    {
        // Arrange
        UserDao user = new(Guid.NewGuid(), "Some User", "Some Password", new byte[] { 0x00, 0x11, 0x22 });

        // Act
        await _sut.AddAsync(user);

        // Assert
        IEnumerable<UserDao> result = await GetAllUsersAsync();

        result.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task ShouldAddNewUserWithAvatarToNonEmpty()
    {
        // Arrange
        List<UserDao> users = await AddBasicUsersAsync(new byte[] { 0x00, 0x11, 0x22 });
        UserDao userIn = new(Guid.NewGuid(), "Some User", "Some Password", new byte[] { 0x00, 0x11, 0x22 });
        users.Add(userIn);

        // Act
        await _sut.AddAsync(userIn);

        // Assert
        IEnumerable<UserDao> result = await GetAllUsersAsync();

        result.Should().HaveCount(4)
            .And.BeEquivalentTo(users, options => options.WithStrictOrdering());
    }

    [Fact]
    public async Task ShouldRefuseNewUserWithAvatarWithSameName()
    {
        // Arrange
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        UserDao user1 = new(Guid.NewGuid(), "Some User", "Password1", new byte[] { 0x00, 0x11, 0x22 });

        string sqlInsert = """
            INSERT INTO Users (Id, Username, Password, Avatar)
            VALUES (@Id, @Username, @Password, @Avatar)
            """;
        await connection.ExecuteAsync(sqlInsert, user1);

        UserDao userIn = new(Guid.NewGuid(), "Some User", "Some Password", new byte[] { 0x00, 0x11, 0x22 });

        Func<Task> f = async () => await _sut.AddAsync(userIn);

        // Act & Assert
        await f.Should().ThrowAsync<SqliteException>();
    }

    [Fact]
    public async Task ShouldRefuseNewUserWithAvatarWithSameId()
    {
        // Arrange
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        UserDao user1 = new(Guid.NewGuid(), "Some User", "Password", new byte[] { 0x00, 0x11, 0x22 });

        string sqlInsert = """
            INSERT INTO Users (Id, Username, Password, Avatar)
            VALUES (@Id, @Username, @Password, @Avatar)
            """;
        await connection.ExecuteAsync(sqlInsert, user1);

        UserDao userIn = new(user1.Id, "User1", "Some Password", new byte[] { 0x00, 0x11, 0x22 });

        Func<Task> f = async () => await _sut.AddAsync(userIn);

        // Act & Assert
        await f.Should().ThrowAsync<SqliteException>();
    }

    [Fact]
    public async Task ShouldDeleteEntryById()
    {
        // Arrange
        List<UserDao> users = await AddBasicUsersAsync();
        Guid id = users[1].Id;
        users.RemoveAt(1);

        // Act
        await _sut.DeleteAsync(id);

        // Assert
        IEnumerable<UserDao> result = await GetAllUsersAsync();

        result.Should().HaveCount(2)
            .And.BeEquivalentTo(users, options => options.WithStrictOrdering());
    }

    [Fact]
    public async Task ShouldDeleteEntryByUsername()
    {
        // Arrange
        List<UserDao> users = await AddBasicUsersAsync();
        string username = users[2].Username;
        users.RemoveAt(2);

        // Act
        await _sut.DeleteAsync(username);

        // Assert
        IEnumerable<UserDao> result = await GetAllUsersAsync();

        result.Should().HaveCount(2)
            .And.BeEquivalentTo(users, options => options.WithStrictOrdering());
    }

    [Fact]
    public async Task ShouldGetUserById()
    {
        // Arrange
        List<UserDao> users = await AddBasicUsersAsync();
        Guid id = users[1].Id;
        UserDao user = users[1];

        // Act
        UserDao result = await _sut.GetUserAsync(id);

        // Assert
        result.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task ShouldGetUserByUsername()
    {
        // Arrange
        List<UserDao> users = await AddBasicUsersAsync();
        string username = users[2].Username;
        UserDao user = users[2];

        // Act
        UserDao result = await _sut.GetUserAsync(username);

        // Assert
        result.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task ShouldGetAllUsers()
    {
        // Arrange
        List<UserDao> users = await AddBasicUsersAsync();

        // Act
        IEnumerable<UserDao> result = await _sut.GetUsersAsync();

        // Assert
        result.Should().HaveCount(users.Count)
            .And.BeEquivalentTo(users);
    }

    [Fact]
    public async Task ShouldUpdateUser()
    {
        // Arrange
        List<UserDao> users = await AddBasicUsersAsync();
        UserDao user = users[1];
        user.Username = "Some username";
        user.Password = "New password";

        // Act
        await _sut.UpdateAsync(user);

        // Assert
        IEnumerable<UserDao> result = await GetAllUsersAsync();
        result.Should().HaveCount(3)
            .And.BeEquivalentTo(new[]
            {
                users[0],
                user,
                users[2]
            });
    }
}
