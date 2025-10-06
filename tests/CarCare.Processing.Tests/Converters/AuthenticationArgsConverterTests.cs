using CarCare.Processing.Converters;
using CarCare.Processing.Models.Communication;
using FluentAssertions;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace CarCare.Processing.Tests.Converters;

public class AuthenticationArgsConverterTests
{
    public readonly AuthenticationArgsConverter _sut;

    public AuthenticationArgsConverterTests()
    {
        _sut = new();
    }

    [Fact]
    public void ShouldCreateValidObject()
    {
        Thread sta = new(() =>
        {
            // Arrange
            Window window = new();
            PasswordBox passBox = new();
            object[] args = new object[] { window, passBox };

            Type target = typeof(AuthenticationArgs);
            object parameter = null!;
            CultureInfo culture = CultureInfo.InvariantCulture;

            // Act
            object result = _sut.Convert(args, target, parameter, culture);

            // Assert
            result.Should().BeOfType<AuthenticationArgs>()
                .Which.Should().BeEquivalentTo(new AuthenticationArgs()
                {
                    Window = window,
                    PasswordBox = passBox
                });
        });
    }

    [Fact]
    public void ShouldFailDueToWindow()
    {
        Thread sta = new(() =>
        {
            // Arrange
            PasswordBox passBox = new();
            object[] args = new object[] { new(), passBox };

            Type target = typeof(AuthenticationArgs);
            object parameter = null!;
            CultureInfo culture = CultureInfo.InvariantCulture;

            Action f = () => _sut.Convert(args, target, parameter, culture);

            // Act & Assert
            f.Should().Throw<ArgumentException>();
        });
    }

    [Fact]
    public void ShouldFailDueToPasswordBox()
    {
        Thread sta = new(() =>
        {
            // Arrange
            Window window = new();
            object[] args = new object[] { window, new() };

            Type target = typeof(AuthenticationArgs);
            object parameter = null!;
            CultureInfo culture = CultureInfo.InvariantCulture;

            Action f = () => _sut.Convert(args, target, parameter, culture);

            // Act & Assert
            f.Should().Throw<ArgumentException>();
        });
    }
}
