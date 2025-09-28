using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using CarCare.Processing.Models;
using CarCare.Processing.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using OneOf;
using System.Linq.Expressions;

namespace CarCare.Processing.Tests.Services;

public class UserServiceTests
{
    private static readonly Guid Id = Guid.NewGuid();
    private const string Username = "Username";
    private const string Password = "password";
    private static readonly byte[] Avatar = new byte[] { 0x11, 0x22, 0x23 };

    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _hasher;
    private readonly UserService _sut;

    public UserServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _hasher = Substitute.For<IPasswordHasher<User>>();
        _sut = new(_userRepository, _hasher);
    }

    private static User GetUser() => new(Id, Username, Password, Avatar);
    private static UserDao GetUserDao() => new(Id, Username, Password, Avatar);

    [Fact]
    public async Task ShouldRequestUserDeleteById()
    {
        // Arrange
        _userRepository.DeleteAsync(Id).Returns(Task.CompletedTask);

        // Act
        await _sut.DeleteAsync(Id);

        // Assert
        await _userRepository.Received(1).DeleteAsync(Id);
    }

    [Fact]
    public async Task ShouldRequestUserDeleteByUsername()
    {
        // Arrange
        _userRepository.DeleteAsync(Username).Returns(Task.CompletedTask);

        // Act
        await _sut.DeleteAsync(Username);

        // Assert
        await _userRepository.Received(1).DeleteAsync(Username);
    }

    [Fact]
    public async Task ShouldGetUserById()
    {
        // Arrange
        UserDao dao = GetUserDao();
        User user = GetUser();
        _userRepository.GetUserAsync(Id).Returns(dao);

        // Act
        User result = await _sut.GetUserAsync(Id);

        // Assert
        await _userRepository.Received(1).GetUserAsync(Id);
        result.Should().BeEquivalentTo(user)
            .And.NotBe(user);
    }

    [Fact]
    public async Task ShouldGetUserByUsername()
    {
        // Arrange
        UserDao dao = GetUserDao();
        User user = GetUser();
        _userRepository.GetUserAsync(Username).Returns(dao);

        // Act
        User result = await _sut.GetUserAsync(Username);

        // Assert
        await _userRepository.Received(1).GetUserAsync(Username);
        result.Should().BeEquivalentTo(user)
            .And.NotBe(user);
    }

    [Fact]
    public async Task ShouldGetUsers()
    {
        // Arrange
        UserDao[] daos = new UserDao[]
        {
            new(Guid.NewGuid(), "Username 1", "Password 1"),
            new(Guid.NewGuid(), "Username 2", "Password 2"),
            new(Guid.NewGuid(), "Username 3", "Password 3"),
        };
        User[] users = new User[]
        {
            new(daos[0].Id, "Username 1", "Password 1"),
            new(daos[1].Id, "Username 2", "Password 2"),
            new(daos[2].Id, "Username 3", "Password 3"),
        };
        _userRepository.GetUsersAsync().Returns(daos);

        // Act
        IEnumerable<User> result = await _sut.GetUsersAsync();

        // Assert
        result.Should().HaveCount(users.Length)
            .And.BeEquivalentTo(users, options => options.WithStrictOrdering());
    }

    [Fact]
    public async Task ShouldSuccessfullyLogUserIn()
    {
        // Arrange
        UserDao dao = GetUserDao();
        User user = GetUser();
        _userRepository.GetUserAsync(Username).Returns(dao);
        _hasher.VerifyHashedPassword(null!, Password, Password).Returns(PasswordVerificationResult.Success);

        // Act
        OneOf<User, LoginError> result = await _sut.LoginAsync(Username, Password);

        // Assert
        await _userRepository.Received().GetUserAsync(Username);
        _hasher.Received().VerifyHashedPassword(null!, Password, Password);

        result.Value.Should().NotBeNull()
            .And.BeAssignableTo<User>()
            .Which.Should().BeEquivalentTo(user)
            .And.NotBe(user);
    }

    [Fact]
    public async Task ShouldFailToLogUserIn()
    {
        // Arrange
        UserDao dao = GetUserDao();
        _userRepository.GetUserAsync(Username).Returns(dao);
        _hasher.VerifyHashedPassword(null!, Password, Password).Returns(PasswordVerificationResult.Failed);

        // Act
        OneOf<User, LoginError> result = await _sut.LoginAsync(Username, Password);

        // Assert
        await _userRepository.Received().GetUserAsync(Username);
        _hasher.Received().VerifyHashedPassword(null!, Password, Password);

        result.Value.Should().NotBeNull()
            .And.BeAssignableTo<LoginError>();
    }

    [Fact]
    public async Task ShouldRequestUserAddition()
    {
        // Arrange
        User user = GetUser();
        UserDao dao = GetUserDao();
        _userRepository.AddAsync(dao).Returns(Task.CompletedTask);

        // Act
        await _sut.RegisterAsync(user);

        // Assert
        await _userRepository.Received().AddAsync(Arg.Is<UserDao>(x =>
            x.Username == Username && x.Password == Password && x.Id == Id && x.Avatar == Avatar));
    }

    [Fact]
    public async Task ShouldRequestUserUpdate()
    {
        // Arrange
        User user = GetUser();
        UserDao dao = GetUserDao();
        _userRepository.UpdateAsync(dao).Returns(Task.CompletedTask);

        // Act
        await _sut.UpdateAsync(user);

        // Assert
        await _userRepository.Received().UpdateAsync(Arg.Is<UserDao>(x =>
            x.Username == Username && x.Password == Password && x.Id == Id && x.Avatar == Avatar));
    }
}
