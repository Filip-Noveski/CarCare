using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models.Communication;
using CarCare.Processing.Models.Core;
using CarCare.Processing.Models.Dto;
using CarCare.Processing.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using OneOf;
using System.Windows.Media.Imaging;

namespace CarCare.Processing.Tests.Services;

public class UserServiceTests
{
    private static readonly Guid Id = Guid.NewGuid();
    private const string Username = "Username";
    private const string Password = "password";
    private static readonly byte[] Avatar = new byte[] { 0x11, 0x22, 0x23 };
    private static readonly BitmapImage AvatarBmp = new();

    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _hasher;
    private readonly IBitmapCreatorService _bitmapService;
    private readonly IUserSession _userSession;
    private readonly IUserSettingsService _userSettingsService;
    private readonly UserService _sut;

    public UserServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _hasher = Substitute.For<IPasswordHasher<User>>();
        _bitmapService = Substitute.For<IBitmapCreatorService>();
        _userSession = Substitute.For<IUserSession>();
        _userSettingsService = Substitute.For<IUserSettingsService>();
        _sut = new(_userRepository, _hasher, _bitmapService, _userSession, _userSettingsService);
    }

    private static User GetUser() => new(Id, Username, Password, AvatarBmp, true);
    private static UserDao GetUserDao() => new(Id, Username, Password, Avatar);

    [Fact]
    public async Task ShouldRequestUserDeleteById()
    {
        // Arrange
        _userRepository.DeleteAsync(Id).Returns(Task.CompletedTask);
        _userSession.IsAuthenticated(Id).Returns(true);
        _userSettingsService.DeleteAsync(Id).Returns(Task.CompletedTask);

        // Act
        await _sut.DeleteAsync(Id);

        // Assert
        await _userRepository.Received(1).DeleteAsync(Id);
        _userSession.Received().IsAuthenticated(Id);
        await _userSettingsService.Received().DeleteAsync(Id);
    }

    [Fact]
    public async Task ShouldRequestUserDeleteByUsername()
    {
        // Arrange
        _userRepository.DeleteAsync(Username).Returns(Task.CompletedTask);
        _userSession.IsAuthenticated(Username).Returns(true);
        UserDao dao = GetUserDao();
        _userRepository.GetUserAsync(Username).Returns(dao);
        _userSettingsService.DeleteAsync(Id).Returns(Task.CompletedTask);

        // Act
        await _sut.DeleteAsync(Username);

        // Assert
        await _userRepository.Received(1).DeleteAsync(Username);
        _userSession.Received().IsAuthenticated(Username);
        await _userRepository.Received().GetUserAsync(Username);
        await _userSettingsService.Received().DeleteAsync(Id);
    }

    [Fact]
    public async Task ShouldRejectUserDeleteById()
    {
        // Arrange
        _userRepository.DeleteAsync(Id).Returns(Task.CompletedTask);
        _userSession.IsAuthenticated(Id).Returns(false);

        // Act
        await _sut.DeleteAsync(Id);

        // Assert
        await _userRepository.DidNotReceive().DeleteAsync(Id);
        _userSession.Received().IsAuthenticated(Id);
    }

    [Fact]
    public async Task ShouldRejectUserDeleteByUsername()
    {
        // Arrange
        _userRepository.DeleteAsync(Username).Returns(Task.CompletedTask);
        _userSession.IsAuthenticated(Username).Returns(false);

        // Act
        await _sut.DeleteAsync(Username);

        // Assert
        await _userRepository.DidNotReceive().DeleteAsync(Username);
        _userSession.Received().IsAuthenticated(Username);
    }

    [Fact]
    public async Task ShouldGetUserById()
    {
        // Arrange
        UserDao dao = GetUserDao();
        User user = GetUser();
        _userRepository.GetUserAsync(Id).Returns(dao);
        _bitmapService.ConvertToBitmap(Avatar).Returns(AvatarBmp);

        // Act
        User result = await _sut.GetUserAsync(Id);

        // Assert
        await _userRepository.Received(1).GetUserAsync(Id);
        _bitmapService.Received().ConvertToBitmap(Avatar);
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
        _bitmapService.ConvertToBitmap(Avatar).Returns(AvatarBmp);

        // Act
        User result = await _sut.GetUserAsync(Username);

        // Assert
        await _userRepository.Received(1).GetUserAsync(Username);
        _bitmapService.Received().ConvertToBitmap(Avatar);
        result.Should().BeEquivalentTo(user)
            .And.NotBe(user);
    }

    [Fact]
    public async Task ShouldGetUsers()
    {
        // Arrange
        UserDao[] daos = new UserDao[]
        {
            new(Guid.NewGuid(), "Username 1", "Password 1", Avatar),
            new(Guid.NewGuid(), "Username 2", "Password 2", Avatar),
            new(Guid.NewGuid(), "Username 3", "Password 3", Avatar)
        };
        User[] users = new User[]
        {
            new(daos[0].Id, "Username 1", "Password 1", AvatarBmp, true),
            new(daos[1].Id, "Username 2", "Password 2", AvatarBmp, true),
            new(daos[2].Id, "Username 3", "Password 3", AvatarBmp, true)
        };
        _userRepository.GetUsersAsync().Returns(daos);
        _bitmapService.ConvertToBitmap(Avatar).Returns(AvatarBmp);

        // Act
        IEnumerable<User> result = await _sut.GetUsersAsync();

        // Assert
        List<User> list = result.ToList();  // to avoid Linq recalculations
        list.Should().HaveCount(users.Length)
            .And.BeEquivalentTo(users, options => options.WithStrictOrdering());
        _bitmapService.Received(3).ConvertToBitmap(Avatar);
        await _userRepository.Received().GetUsersAsync();
    }

    [Fact]
    public async Task ShouldGetUsersWithoutAvatar()
    {
        // Arrange
        UserDao[] daos = new UserDao[]
        {
            new(Guid.NewGuid(), "Username 1", "Password 1"),
            new(Guid.NewGuid(), "Username 2", "Password 2"),
            new(Guid.NewGuid(), "Username 3", "Password 3")
        };
        User[] users = new User[]
        {
            new(daos[0].Id, "Username 1", "Password 1", AvatarBmp, false),
            new(daos[1].Id, "Username 2", "Password 2", AvatarBmp, false),
            new(daos[2].Id, "Username 3", "Password 3", AvatarBmp, false)
        };
        _userRepository.GetUsersAsync().Returns(daos);
        _bitmapService.GetGenericAvatar(Arg.Any<string>()).Returns(AvatarBmp);

        // Act
        IEnumerable<User> result = await _sut.GetUsersAsync();

        // Assert
        List<User> list = result.ToList();  // to avoid Linq recalculations
        list.Should().HaveCount(users.Length)
            .And.BeEquivalentTo(users, options => options.WithStrictOrdering());
        _bitmapService.Received(3).GetGenericAvatar(Arg.Any<string>());
        await _userRepository.Received().GetUsersAsync();
    }

    [Fact]
    public async Task ShouldSuccessfullyLogUserIn()
    {
        // Arrange
        UserDao dao = GetUserDao();
        User user = GetUser();
        _userRepository.GetUserAsync(Username).Returns(dao);
        _hasher.VerifyHashedPassword(null!, Password, Password).Returns(PasswordVerificationResult.Success);
        _bitmapService.ConvertToBitmap(Avatar).Returns(AvatarBmp);
        _userSession.LoginUser(Arg.Any<UserDto>());

        // Act
        OneOf<User, LoginError> result = await _sut.LoginAsync(Username, Password);

        // Assert
        await _userRepository.Received().GetUserAsync(Username);
        _hasher.Received().VerifyHashedPassword(null!, Password, Password);
        _bitmapService.Received().ConvertToBitmap(Avatar);
        _userSession.Received().LoginUser(Arg.Any<UserDto>());

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
        _userSession.DidNotReceive().LoginUser(Arg.Any<UserDto>());

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
        _bitmapService.ConvertToBinary(AvatarBmp).Returns(Avatar);
        _userSession.LoginUser(Arg.Any<UserDto>());
        _userSettingsService.AddAsync(Arg.Any<UserSettings>()).Returns(Task.CompletedTask);

        // Act
        await _sut.RegisterAsync(user);

        // Assert
        await _userRepository.Received().AddAsync(Arg.Is<UserDao>(x =>
            x.Username == Username && x.Password == Password && x.Id == Id && x.Avatar == Avatar));
        _bitmapService.Received().ConvertToBinary(AvatarBmp);
        _userSession.Received().LoginUser(Arg.Any<UserDto>());
        await _userSettingsService.Received().AddAsync(Arg.Is<UserSettings>(
            x => x.UserId == user.Id));
    }

    [Fact]
    public async Task ShouldRequestUserUpdate()
    {
        // Arrange
        User user = GetUser();
        UserDao dao = GetUserDao();
        _userRepository.UpdateAsync(dao).Returns(Task.CompletedTask);
        _bitmapService.ConvertToBinary(AvatarBmp).Returns(Avatar);
        _userSession.IsAuthenticated(user).Returns(true);

        // Act
        await _sut.UpdateAsync(user);

        // Assert
        await _userRepository.Received().UpdateAsync(Arg.Is<UserDao>(x =>
            x.Username == Username && x.Password == Password && x.Id == Id && x.Avatar == Avatar));
        _bitmapService.Received().ConvertToBinary(AvatarBmp);
        _userSession.Received().IsAuthenticated(user);
    }

    [Fact]
    public async Task ShouldRejectUserUpdate()
    {
        // Arrange
        User user = GetUser();
        UserDao dao = GetUserDao();
        _userRepository.UpdateAsync(dao).Returns(Task.CompletedTask);
        _bitmapService.ConvertToBinary(AvatarBmp).Returns(Avatar);
        _userSession.IsAuthenticated(user).Returns(false);

        // Act
        await _sut.UpdateAsync(user);

        // Assert
        await _userRepository.DidNotReceive().UpdateAsync(Arg.Is<UserDao>(x =>
            x.Username == Username && x.Password == Password && x.Id == Id && x.Avatar == Avatar));
        _bitmapService.DidNotReceive().ConvertToBinary(AvatarBmp);
        _userSession.Received().IsAuthenticated(user);
    }
}
