using CarCare.Processing.Contexts;
using FluentAssertions;

namespace CarCare.Processing.Tests.Contexts;

public class AuthenticationContextTests
{
    private readonly AuthenticationContext _sut;

    public AuthenticationContextTests()
    {
        _sut = new();
    }

    [Fact]
    public void ShouldRaiseRegisterRequestEvent()
    {
        // Arrange
        bool called = false;
        EventHandler a = (s, e) =>
        {
            called = true;
        };
        _sut.RegisterRequested += a;

        // Act
        _sut.OnRegisterRequested();

        // Assert
        called.Should().BeTrue();
    }

    [Fact]
    public void ShouldRaiseLoginRequestEvent()
    {
        // Arrange
        bool called = false;
        EventHandler a = (s, e) =>
        {
            called = true;
        };
        _sut.LoginRequested += a;

        // Act
        _sut.OnLoginRequested();

        // Assert
        called.Should().BeTrue();
    }
}
