using CarCare.Processing.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;

namespace CarCare.Processing.Tests.Models;

public class UserTests
{
    private readonly PasswordHasher<User> _hasher;

    public UserTests()
    {
        _hasher = new();
    }

    [Fact]
    public void ShouldCreateUserWithHashedPasswordAndNoAvatar()
    {
        // Arrange
        string username = "Some username";
        string password = "Some password";

        // Act
        User user = User.Create(username, password, _hasher);

        // Assert
        PasswordVerificationResult passVerification = _hasher.VerifyHashedPassword(null!, user.Password, password);

        user.Username.Should().Be(username);
        user.Avatar.Should().BeNull();
        passVerification.Should().Be(PasswordVerificationResult.Success);
    }

    [Fact]
    public void ShouldCreateUserWithHashedPasswordAndAvatar()
    {
        // Arrange
        string username = "Some username";
        string password = "Some password";
        byte[] avatar = new byte[] { 0x02, 0x13, 0x65 };

        // Act
        User user = User.Create(username, password, avatar, _hasher);

        // Assert
        PasswordVerificationResult passVerification = _hasher.VerifyHashedPassword(null!, user.Password, password);

        user.Username.Should().Be(username);
        user.Avatar.Should().BeEquivalentTo(avatar);
        passVerification.Should().Be(PasswordVerificationResult.Success);
    }
}
