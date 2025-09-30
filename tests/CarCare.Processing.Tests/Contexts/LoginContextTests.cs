using CarCare.Processing.Contexts;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Models;
using FluentAssertions;
using NSubstitute;
using System.Windows;

namespace CarCare.Processing.Tests.Contexts;

public class LoginContextTests
{
    private readonly IUserService _userService;
    private readonly INavigationService _navigationService;
    private readonly LoginContext _sut;

    public LoginContextTests()
    {
        _userService = Substitute.For<IUserService>();
        _navigationService = Substitute.For<INavigationService>();
        _sut = new(_userService, _navigationService);
    }


    [Fact]
    public async Task ShouldFailDueToIncorrectParamter()
    {
        // Arrange
        object input = new();

        // Act
        _sut.LoginCommand.Execute(input);

        // Assert
        _sut.Error.Should().NotBe("");
        _navigationService.DidNotReceive().NavigateTo<IDashboardContext>();
        _navigationService.DidNotReceive().NavigateTo<IDashboardContext>(Arg.Any<Window>());
        await _userService.DidNotReceive().LoginAsync(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task ShouldDenyLogin()
    {
        _userService.LoginAsync(Arg.Any<string>(), Arg.Any<string>())
            .Returns(new LoginError("Invalid Password let's say"));

        Thread sta = new(() =>
        {
            // Arrange
            AuthenticationArgs args = new()
            {
                Window = null!,
                PasswordBox = new()
            };
            _sut.Username = "Username";
            args.PasswordBox.Password = "Password";

            // Act
            _sut.LoginCommand.Execute(args);
        });
        sta.SetApartmentState(ApartmentState.STA);
        sta.Start();
        sta.Join();

        // Assert
        await _userService.Received().LoginAsync("Username", "Password");
        _sut.Error.Should().Be("Invalid Password let's say");
        _navigationService.DidNotReceive().NavigateTo<IDashboardContext>(Arg.Any<Window>());
    }

    [Fact]
    public async Task ShouldApproveLogin()
    {
        _userService.LoginAsync(Arg.Any<string>(), Arg.Any<string>())
            .Returns(new User(Guid.NewGuid(), "Username", "Password"));

        Thread sta = new(() =>
        {
            // Arrange
            AuthenticationArgs args = new()
            {
                Window = new(),
                PasswordBox = new()
            };
            _sut.Username = "Username";
            args.PasswordBox.Password = "Password";

            // Act
            _sut.LoginCommand.Execute(args);

            // Assert navigation service (otherwise would be out of scope)
            _navigationService.Received().NavigateTo<IDashboardContext>(args.Window);
        });
        sta.SetApartmentState(ApartmentState.STA);
        sta.Start();
        sta.Join();

        // Assert
        await _userService.Received().LoginAsync("Username", "Password");
        _sut.Error.Should().Be("");
    }
}
