using CarCare.Processing.Contexts;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Models.Communication;
using CarCare.Processing.Models.Core;
using FluentAssertions;
using NSubstitute;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CarCare.Processing.Tests.Contexts;

public class LoginContextTests
{
    private readonly IUserService _userService;
    private readonly INavigationService _navigationService;
    private readonly IAuthenticationContext _authenticationContext;
    private readonly LoginContext _sut;

    public LoginContextTests()
    {
        _userService = Substitute.For<IUserService>();
        _navigationService = Substitute.For<INavigationService>();
        _authenticationContext = Substitute.For<AuthenticationContext>();
        _sut = new(_userService, _navigationService, _authenticationContext);
    }


    [Fact]
    public async Task LoginShouldFailDueToIncorrectParamter()
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
    public async Task LoginShouldBeDenied()
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
            .Returns(new User(Guid.NewGuid(), "Username", "Password", new BitmapImage(), true));

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

    [Fact]
    public void ShouldNavigateToRegister()
    {
        // Arrange
        _authenticationContext.OnRegisterRequested();

        // Act
        _sut.RegisterCommand.Execute(null);

        // Assert
        _authenticationContext.Received().OnRegisterRequested();
    }

    [Fact]
    public void ShouldSelectUser()
    {
        // Arrange
        BitmapImage avatar = new();
        _sut.Users.Add(new(Guid.NewGuid(), "Username", avatar));

        // Act
        _sut.SelectUserCommand.Execute("Username");

        // Assert
        _sut.Username.Should().Be("Username");
        _sut.Avatar.Should().Be(avatar);
    }

    [Fact]
    public void ShouldFailToSelectUser()
    {
        // Act
        _sut.SelectUserCommand.Execute(new object());

        // Assert
        _sut.Error.Should().NotBe("");
    }

    [Fact]
    public void ShouldClearUsernameAndAvatar()
    {
        // Arrange
        _sut.Username = "asdsad";
        _sut.Avatar = new BitmapImage();

        // Act
        _sut.ReturnToUsersListCommand.Execute(null);

        // Assert
        _sut.Username.Should().Be("");
        _sut.Avatar.Should().BeNull();
    }

    [Fact]
    public void UsernameChangeShouldRaiseEvent()
    {
        // Arrange
        bool called = false;
        EventHandler a = (s, e) =>
        {
            called = true;
        };
        _sut.UsernameChanged += a;

        // Act
        _sut.Username = "Hello";

        // Assert
        called.Should().BeTrue();
    }
}
