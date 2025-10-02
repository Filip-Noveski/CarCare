using CarCare.Processing.Models;
using CarCare.Processing.Sessions;
using FluentAssertions;
using System.Reflection;

namespace CarCare.Processing.Tests.Sessions;

public class UserSessionTests
{
    private readonly UserSession _sut;

    private readonly UserDto _dto;

    public UserSessionTests()
    {
        _sut = new();
        _dto = new(Guid.NewGuid(), "Username", new());
    }

    private void ForceSetUser()
    {
        PropertyInfo userProp = typeof(UserSession).GetProperty(nameof(UserSession.User))!;
        userProp.SetValue(_sut, _dto);

        // ensure that property has been set correctly
        _sut.User.Should().NotBeNull()
            .And.Be(_dto);
    }

    [Fact]
    public void ShouldLogUserIn()
    {
        // Act
        _sut.LoginUser(_dto);

        // Assert
        UserDto? loggedIn = _sut.User;
        loggedIn.Should().NotBeNull()
            .And.BeEquivalentTo(_dto);
    }

    [Fact]
    public void ShouldNotLogUserInIfSomeoneIsAlreadyLoggedIn()
    {
        // Arrange
        ForceSetUser();
        UserDto newUser = new(Guid.NewGuid(), "Some other user", new());

        // Act
        _sut.LoginUser(newUser);

        // Assert
        UserDto? loggedIn = _sut.User;
        loggedIn.Should().NotBeNull()
            .And.BeEquivalentTo(_dto)
            .And.NotBeEquivalentTo(newUser);
    }

    [Fact]
    public void ShouldLogUserOut()
    {
        // Arrange
        ForceSetUser();

        // Act
        _sut.LogoutUser();

        // Assert
        _sut.User.Should().BeNull();
    }

    [Fact]
    public void ShouldReturnUserAuthenticatedByObject()
    {
        // Arrange
        ForceSetUser();
        User user = new(_dto.Id, _dto.Username, "Irrelevant", _dto.Avatar, false);

        // Act
        bool result = _sut.IsAuthenticated(user);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnUserAuthenticatedById()
    {
        // Arrange
        ForceSetUser();
        Guid id = _dto.Id;

        // Act
        bool result = _sut.IsAuthenticated(id);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnUserAuthenticatedByUsername()
    {
        // Arrange
        ForceSetUser();
        string username = _dto.Username;

        // Act
        bool result = _sut.IsAuthenticated(username);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnUserNotAuthenticatedByObject()
    {
        // Arrange
        ForceSetUser();
        User user = new(Guid.NewGuid(), "Some new username", "Irrelevant", _dto.Avatar, false);

        // Act
        bool result = _sut.IsAuthenticated(user);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnUserNotAuthenticatedById()
    {
        // Arrange
        ForceSetUser();
        Guid id = Guid.NewGuid();

        // Act
        bool result = _sut.IsAuthenticated(id);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnUserNotAuthenticatedByUsername()
    {
        // Arrange
        ForceSetUser();
        string username = "Whatever";

        // Act
        bool result = _sut.IsAuthenticated(username);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnUserNotAuthenticatedByObjectIfNull()
    {
        // Arrange
        User user = new(Guid.NewGuid(), "Some new username", "Irrelevant", _dto.Avatar, false);

        // Act
        bool result = _sut.IsAuthenticated(user);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnUserNotAuthenticatedByIdIfNull()
    {
        // Arrange
        Guid id = Guid.NewGuid();

        // Act
        bool result = _sut.IsAuthenticated(id);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnUserNotAuthenticatedByUsernameIfNull()
    {
        // Arrange
        string username = "Whatever";

        // Act
        bool result = _sut.IsAuthenticated(username);

        // Assert
        result.Should().BeFalse();
    }
}
