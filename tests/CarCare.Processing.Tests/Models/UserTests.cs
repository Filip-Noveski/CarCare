using CarCare.Persistence.Models;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Models.Core;
using CarCare.Processing.Models.Dto;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using System.Windows.Media.Imaging;

namespace CarCare.Processing.Tests.Models;

public class UserTests
{
    private readonly PasswordHasher<User> _hasher;
    private readonly IBitmapCreatorService _bitmapCreator;

    public UserTests()
    {
        _hasher = new();
        _bitmapCreator = Substitute.For<IBitmapCreatorService>();
    }

    [Fact]
    public void ShouldCreateUserWithHashedPasswordAndNoAvatar()
    {
        // Arrange
        string username = "Some username";
        string password = "Some password";
        BitmapImage avatar = new();

        _bitmapCreator.GetGenericAvatar(username).Returns(avatar);

        // Act
        User user = User.Create(username, password, _hasher, _bitmapCreator);

        // Assert
        PasswordVerificationResult passVerification = _hasher.VerifyHashedPassword(null!, user.Password, password);

        user.Username.Should().Be(username);
        user.Avatar.Should().Be(avatar);
        user.HasCustomAvatar.Should().BeFalse();
        passVerification.Should().Be(PasswordVerificationResult.Success);

        _bitmapCreator.Received().GetGenericAvatar(username);
    }

    [Fact]
    public void ShouldCreateUserWithHashedPasswordAndAvatar()
    {
        // Arrange
        string username = "Some username";
        string password = "Some password";
        byte[] avatar = new byte[] { 0x02, 0x13, 0x65 };
        BitmapImage bmp = new();

        _bitmapCreator.ConvertToBitmap(avatar).Returns(bmp);

        // Act
        User user = User.Create(username, password, avatar, _hasher, _bitmapCreator);

        // Assert
        PasswordVerificationResult passVerification = _hasher.VerifyHashedPassword(null!, user.Password, password);

        user.Username.Should().Be(username);
        user.Avatar.Should().Be(bmp);
        user.HasCustomAvatar.Should().BeTrue();
        passVerification.Should().Be(PasswordVerificationResult.Success);

        _bitmapCreator.Received().ConvertToBitmap(avatar);
    }

    [Fact]
    public void ShouldCreateUserWithHashedPasswordAndDirectlyPassesdAvatar()
    {
        // Arrange
        string username = "Some username";
        string password = "Some password";
        BitmapImage avatar = new();

        // Act
        User user = User.Create(username, password, avatar, _hasher);

        // Assert
        PasswordVerificationResult passVerification = _hasher.VerifyHashedPassword(null!, user.Password, password);

        user.Username.Should().Be(username);
        user.Avatar.Should().Be(avatar);
        user.HasCustomAvatar.Should().BeTrue();
        passVerification.Should().Be(PasswordVerificationResult.Success);
    }

    [Fact]
    public void ShouldConvertToDto()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        string username = "Some username";
        string password = "Some password";
        BitmapImage avatar = new();
        User user = new(id, username, password, avatar, true);

        // Act
        UserDto dto = user.ToDto();

        // Assert
        dto.Id.Should().Be(id);
        dto.Username.Should().Be(username);
        dto.Avatar.Should().Be(avatar);
    }

    [Fact]
    public void ShouldConvertToDaoWithAvatar()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        string username = "Some username";
        string password = "Some password";
        BitmapImage avatar = new();
        User user = new(id, username, password, avatar, true);
        byte[] avatarBytes = Array.Empty<byte>();

        _bitmapCreator.ConvertToBinary(avatar).Returns(avatarBytes);

        // Act
        UserDao dao = user.ToDao(_bitmapCreator);

        // Assert
        dao.Id.Should().Be(id);
        dao.Username.Should().Be(username);
        dao.Password.Should().Be(password);
        dao.Avatar.Should().BeEquivalentTo(avatarBytes);

        _bitmapCreator.Received().ConvertToBinary(avatar);
    }

    [Fact]
    public void ShouldConvertToDaoWithoutAvatar()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        string username = "Some username";
        string password = "Some password";
        BitmapImage avatar = new();
        User user = new(id, username, password, avatar, false);

        // Act
        UserDao dao = user.ToDao(_bitmapCreator);

        // Assert
        dao.Id.Should().Be(id);
        dao.Username.Should().Be(username);
        dao.Password.Should().Be(password);
        dao.Avatar.Should().BeNull();
    }
}
