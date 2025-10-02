using CarCare.Processing.Contexts;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CarCare.Processing.Tests.Contexts;

public class RegisterContextTests
{
    private readonly IUserService _userService;
    private readonly IPasswordHasher<User> _hasher;
    private readonly INavigationService _navigationService;
    private readonly IBitmapCreatorService _bitmapService;
    private readonly IAuthenticationContext _authenticationContext;
    private readonly IFileDialogueService _dialogueService;
    private readonly RegisterContext _sut;

    public RegisterContextTests()
    {
        _userService = Substitute.For<IUserService>();
        _hasher = Substitute.For<IPasswordHasher<User>>();
        _navigationService = Substitute.For<INavigationService>();
        _bitmapService = Substitute.For<IBitmapCreatorService>();
        _authenticationContext = Substitute.For<IAuthenticationContext>();
        _dialogueService = Substitute.For<IFileDialogueService>();
        _sut = new(
            _userService,
            _hasher,
            _navigationService,
            _bitmapService,
            _authenticationContext,
            _dialogueService);
    }

    [Fact]
    public async Task ShouldFailDueToIncorrectParamter()
    {
        // Arrange
        object input = new();

        // Act
        _sut.RegisterCommand.Execute(input);

        // Assert
        _sut.Error.Should().NotBe("");
        _navigationService.DidNotReceive().NavigateTo<IDashboardContext>();
        _navigationService.DidNotReceive().NavigateTo<IDashboardContext>(Arg.Any<Window>());
        await _userService.DidNotReceive().RegisterAsync(Arg.Any<User>());
        _hasher.DidNotReceive().HashPassword(Arg.Any<User>(), Arg.Any<string>());
    }

    [Fact]
    public async Task ShouldFailDueToWhitespaceUsername()
    {
        // Arrange
        AuthenticationArgs args = new()
        {
            Window = null!,
            PasswordBox = null!
        };
        _sut.Username = "   ";

        // Act
        _sut.RegisterCommand.Execute(args);

        // Assert
        _sut.Error.Should().NotBe("")
            .And.Contain("username")
            .And.Contain("whitespace");
        _navigationService.DidNotReceive().NavigateTo<IDashboardContext>();
        _navigationService.DidNotReceive().NavigateTo<IDashboardContext>(Arg.Any<Window>());
        await _userService.DidNotReceive().RegisterAsync(Arg.Any<User>());
        _hasher.DidNotReceive().HashPassword(Arg.Any<User>(), Arg.Any<string>());
    }

    [Fact]
    public async Task ShouldFailDueToWhitespacePassword()
    {
        // Arrange and Act wrapped in Thread as components require apartment state STA
        Thread sta = new(() =>
        {
            // Arrange
            AuthenticationArgs args = new()
            {
                Window = null!,
                PasswordBox = new()
            };
            _sut.Username = "Username";
            args.PasswordBox.Password = "   ";

            // Act
            _sut.RegisterCommand.Execute(args);
        });
        sta.SetApartmentState(ApartmentState.STA);
        sta.Start();
        sta.Join();
        
        // Assert
        _sut.Error.Should().NotBe("")
            .And.Contain("password")
            .And.Contain("whitespace");
        _navigationService.DidNotReceive().NavigateTo<IDashboardContext>();
        _navigationService.DidNotReceive().NavigateTo<IDashboardContext>(Arg.Any<Window>());
        await _userService.DidNotReceive().RegisterAsync(Arg.Any<User>());
        _hasher.DidNotReceive().HashPassword(Arg.Any<User>(), Arg.Any<string>());
    }

    [Fact]
    public async Task ShouldRequestRegisterWithoutAvatar()
    {
        Thread sta = new(() =>
        {
            // Arrange
            _userService.RegisterAsync(Arg.Any<User>()).Returns(Task.CompletedTask);
            _hasher.HashPassword(Arg.Any<User>(), "Password").Returns("Hashed");
            _navigationService.NavigateTo<IDashboardContext>(Arg.Any<Window>());

            AuthenticationArgs args = new()
            {
                Window = new(),
                PasswordBox = new()
            };
            _sut.Username = "Username";
            args.PasswordBox.Password = "Password";
            _sut.Avatar = null;

            // Act
            _sut.RegisterCommand.Execute(args);

            // Assert navigation service (otherwise would be out of scope)
            _navigationService.Received().NavigateTo<IDashboardContext>(args.Window);
        });
        sta.SetApartmentState(ApartmentState.STA);
        sta.Start();
        sta.Join();

        // Assert
        _sut.Error.Should().Be("");
        await _userService.Received().RegisterAsync(Arg.Is<User>(x => 
            x.Username == "Username" && x.Password == "Hashed" && x.Avatar == null));
        _hasher.Received().HashPassword(Arg.Any<User>(), "Password");
    }

    [Fact]
    public async Task ShouldRequestRegisterWithAvatar()
    {
        BitmapImage bmp = new();
        Thread sta = new(() =>
        {
            // Arrange
            _userService.RegisterAsync(Arg.Any<User>()).Returns(Task.CompletedTask);
            _hasher.HashPassword(Arg.Any<User>(), "Password").Returns("Hashed");
            _navigationService.NavigateTo<IDashboardContext>(Arg.Any<Window>());

            AuthenticationArgs args = new()
            {
                Window = new(),
                PasswordBox = new()
            };
            _sut.Username = "Username";
            args.PasswordBox.Password = "Password";
            _sut.Avatar = bmp;

            // Act
            _sut.RegisterCommand.Execute(args);

            // Assert navigation service (otherwise would be out of scope)
            _navigationService.Received().NavigateTo<IDashboardContext>(args.Window);
        });
        sta.SetApartmentState(ApartmentState.STA);
        sta.Start();
        sta.Join();

        // Assert
        _sut.Error.Should().Be("");
        await _userService.Received().RegisterAsync(Arg.Is<User>(x => 
            x.Username == "Username" && x.Password == "Hashed" && x.Avatar == bmp));
        _hasher.Received().HashPassword(Arg.Any<User>(), "Password");
    }

    [Fact]
    public void ShouldRequestOpenFileDialogueForImage()
    {
        // Arrange
        _dialogueService.GetImageFile().Returns(new BitmapImage());

        // Act
        _sut.ChooseAvatarCommand.Execute(null);

        // Assert
        _dialogueService.Received().GetImageFile();
    }

    [Fact]
    public void ShouldRequestLogin()
    {
        // Arrange
        _authenticationContext.OnLoginRequested();

        // Act
        _sut.NavigateToLoginCommand.Execute(null);

        // Assert
        _authenticationContext.Received().OnLoginRequested();
    }
}
