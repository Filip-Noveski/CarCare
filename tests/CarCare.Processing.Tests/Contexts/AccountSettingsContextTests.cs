using CarCare.Processing.Contexts;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models.Communication;
using CarCare.Processing.Models.Core;
using CarCare.Processing.Models.Dto;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using System.Windows.Media.Imaging;

namespace CarCare.Processing.Tests.Contexts;

public class AccountSettingsContextTests
{
    private readonly UserDto _dto = new(Guid.NewGuid(), "Username", new());

    private readonly IUserService _userService;
    private readonly IUserSession _userSession;
    private readonly IPasswordHasher<User> _hasher;
    private readonly IFileDialogueService _fileService;
    private readonly IBitmapCreatorService _bitmapCreator;
    private readonly AccountSettingsContext _sut;

    public AccountSettingsContextTests()
    {
        _userService = Substitute.For<IUserService>();
        _userSession = Substitute.For<IUserSession>();
        _hasher = Substitute.For<IPasswordHasher<User>>();
        _fileService = Substitute.For<IFileDialogueService>();
        _bitmapCreator = Substitute.For<IBitmapCreatorService>();
        _userSession.User.Returns(_dto);
        _sut = new(_userService, _userSession, _hasher, _fileService, _bitmapCreator);
    }

    [Fact]
    public async Task ShouldRequestUsernameUpdateWithCustomAvatar()
    {
        // Arrange
        _sut.Username = "Thing";
        _userSession.User.Returns(_dto);
        _userService.GetUserAsync(_dto.Id).Returns(new User(_dto.Id, _dto.Username, "Pass", _dto.Avatar, true));
        _userService.UpdateAsync(Arg.Any<User>()).Returns(Task.CompletedTask);

        // Act
        _sut.UpdateUsernameCommand.Execute(null);

        // Assert
        await _userService.Received().GetUserAsync(_dto.Id);
        _bitmapCreator.DidNotReceive().GetGenericAvatar(Arg.Any<string>());
        await _userService.Received().UpdateAsync(Arg.Is<User>(
            u => u.Id == _dto.Id
            && u.Username == "Thing"
            && u.Password == "Pass"
            && u.Avatar == _dto.Avatar
            && u.HasCustomAvatar == true));
        _dto.Username.Should().Be("Thing");
        _sut.UsernameChangeError.Should().BeEmpty();
        _sut.UsernameChangeSuccess.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ShouldRequestUsernameUpdateWithoutCustomAvatar()
    {
        // Arrange
        _sut.Username = "Thing";
        BitmapImage bmp = new();
        _userSession.User.Returns(_dto);
        _userService.GetUserAsync(_dto.Id).Returns(new User(_dto.Id, _dto.Username, "Pass", _dto.Avatar, false));
        _userService.UpdateAsync(Arg.Any<User>()).Returns(Task.CompletedTask);
        _bitmapCreator.GetGenericAvatar("Thing").Returns(bmp);

        // Act
        _sut.UpdateUsernameCommand.Execute(null);

        // Assert
        await _userService.Received().GetUserAsync(_dto.Id);
        _bitmapCreator.Received().GetGenericAvatar("Thing");
        await _userService.Received().UpdateAsync(Arg.Is<User>(
            u => u.Id == _dto.Id
            && u.Username == "Thing"
            && u.Password == "Pass"
            && u.Avatar == bmp
            && u.HasCustomAvatar == false));
        _dto.Username.Should().Be("Thing");
        _sut.UsernameChangeError.Should().BeEmpty();
        _sut.UsernameChangeSuccess.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ShouldNotRequestUsernameUpdateWithNullUser()
    {
        // Arrange
        _sut.Username = "Thing";
        _userSession.User.Returns((UserDto?)null);

        // Act
        _sut.UpdateUsernameCommand.Execute(null);

        // Assert
        await _userService.DidNotReceive().GetUserAsync(Arg.Any<Guid>());
        _bitmapCreator.DidNotReceive().GetGenericAvatar(Arg.Any<string>());
        await _userService.DidNotReceive().UpdateAsync(Arg.Any<User>());
        _dto.Username.Should().Be("Username");
        _sut.UsernameChangeError.Should().BeEmpty();
        _sut.UsernameChangeSuccess.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldNotRequestUsernameUpdateWithEmptyUsername()
    {
        // Arrange
        _sut.Username = "  ";
        _userSession.User.Returns(_dto);

        // Act
        _sut.UpdateUsernameCommand.Execute(null);

        // Assert
        await _userService.DidNotReceive().GetUserAsync(Arg.Any<Guid>());
        _bitmapCreator.DidNotReceive().GetGenericAvatar(Arg.Any<string>());
        await _userService.DidNotReceive().UpdateAsync(Arg.Any<User>());
        _dto.Username.Should().Be("Username");
        _sut.UsernameChangeError.Should().NotBeEmpty();
        _sut.UsernameChangeSuccess.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldRequestPasswordUpdate()
    {
        string oldPass = "old";
        string newPass = "new";
        string hash = "hash";
        Thread sta = new(() =>
        {
            // Arrange
            _userSession.User.Returns(_dto);
            UpdatePasswordArgs args = new()
            {
                OldPasswordBox = new()
                {
                    Password = oldPass
                },
                NewPasswordBox = new()
                {
                    Password = newPass
                },
                ConfPasswordBox = new()
                {
                    Password = newPass
                }
            };
            _userService.LoginAsync(_dto.Username, oldPass).Returns(
                new User(_dto.Id, _dto.Username, oldPass, _dto.Avatar, false));
            _userService.UpdateAsync(Arg.Any<User>()).Returns(Task.CompletedTask);
            _userService.GetUserAsync(_dto.Id).Returns(
                new User(_dto.Id, _dto.Username, oldPass, _dto.Avatar, false));
            _hasher.HashPassword(Arg.Any<User>(), newPass).Returns(hash);

            // Act
            _sut.UpdatePasswordCommand.Execute(args);
        });

        sta.SetApartmentState(ApartmentState.STA);
        sta.Start();
        sta.Join();

        // Assert
        await _userService.Received().LoginAsync(_dto.Username, oldPass);
        await _userService.Received().GetUserAsync(_dto.Id);
        await _userService.Received().UpdateAsync(Arg.Is<User>(
            u => u.Id == _dto.Id
            && u.Username == _dto.Username
            && u.Password == hash
            && u.Avatar == _dto.Avatar
            && u.HasCustomAvatar == false));
        _hasher.Received().HashPassword(Arg.Any<User>(), newPass);
        _sut.PasswordChangeError.Should().BeEmpty();
        _sut.PasswordChangeSuccess.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ShouldNotRequestPasswordUpdateWithWrongConfirmation()
    {
        string oldPass = "old";
        string newPass = "new";
        string confPass = "conf";
        Thread sta = new(() =>
        {
            // Arrange
            _userSession.User.Returns(_dto);
            UpdatePasswordArgs args = new()
            {
                OldPasswordBox = new()
                {
                    Password = oldPass
                },
                NewPasswordBox = new()
                {
                    Password = newPass
                },
                ConfPasswordBox = new()
                {
                    Password = confPass
                }
            };
            _userService.LoginAsync(_dto.Username, oldPass).Returns(
                new User(_dto.Id, _dto.Username, oldPass, _dto.Avatar, false));

            // Act
            _sut.UpdatePasswordCommand.Execute(args);
        });

        sta.SetApartmentState(ApartmentState.STA);
        sta.Start();
        sta.Join();

        // Assert
        await _userService.Received().LoginAsync(_dto.Username, oldPass);
        await _userService.DidNotReceive().UpdateAsync(Arg.Any<User>());
        _hasher.DidNotReceive().HashPassword(Arg.Any<User>(), Arg.Any<string>());
        _sut.PasswordChangeError.Should().NotBeEmpty();
        _sut.PasswordChangeSuccess.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldNotRequestPasswordUpdateWithEmptyPassword()
    {
        string oldPass = "old";
        string newPass = "";
        Thread sta = new(() =>
        {
            // Arrange
            _userSession.User.Returns(_dto);
            UpdatePasswordArgs args = new()
            {
                OldPasswordBox = new()
                {
                    Password = oldPass
                },
                NewPasswordBox = new()
                {
                    Password = newPass
                },
                ConfPasswordBox = new()
                {
                    Password = newPass
                }
            };
            _userService.LoginAsync(_dto.Username, oldPass).Returns(
                new User(_dto.Id, _dto.Username, oldPass, _dto.Avatar, false));

            // Act
            _sut.UpdatePasswordCommand.Execute(args);
        });

        sta.SetApartmentState(ApartmentState.STA);
        sta.Start();
        sta.Join();

        // Assert
        await _userService.Received().LoginAsync(_dto.Username, oldPass);
        await _userService.DidNotReceive().UpdateAsync(Arg.Any<User>());
        _hasher.DidNotReceive().HashPassword(Arg.Any<User>(), Arg.Any<string>());
        _sut.PasswordChangeError.Should().NotBeEmpty();
        _sut.PasswordChangeSuccess.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldNotRequestPasswordUpdateWithFailedLogin()
    {
        string oldPass = "old";
        string newPass = "new";
        Thread sta = new(() =>
        {
            // Arrange
            _userSession.User.Returns(_dto);
            UpdatePasswordArgs args = new()
            {
                OldPasswordBox = new()
                {
                    Password = oldPass
                },
                NewPasswordBox = new()
                {
                    Password = newPass
                },
                ConfPasswordBox = new()
                {
                    Password = newPass
                }
            };
            _userService.LoginAsync(_dto.Username, oldPass).Returns(
                new LoginError("Error"));

            // Act
            _sut.UpdatePasswordCommand.Execute(args);
        });

        sta.SetApartmentState(ApartmentState.STA);
        sta.Start();
        sta.Join();

        // Assert
        await _userService.Received().LoginAsync(_dto.Username, oldPass);
        await _userService.DidNotReceive().UpdateAsync(Arg.Any<User>());
        _hasher.DidNotReceive().HashPassword(Arg.Any<User>(), Arg.Any<string>());
        _sut.PasswordChangeError.Should().NotBeEmpty();
        _sut.PasswordChangeSuccess.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldNotRequestPasswordUpdateWithEmptyArgs()
    {
        // Arrange
        _userSession.User.Returns(_dto);
        object args = new();

        // Act
        _sut.UpdatePasswordCommand.Execute(args);

        // Assert
        await _userService.DidNotReceive().LoginAsync(Arg.Any<string>(), Arg.Any<string>());
        await _userService.DidNotReceive().UpdateAsync(Arg.Any<User>());
        _hasher.DidNotReceive().HashPassword(Arg.Any<User>(), Arg.Any<string>());
    }

    [Fact]
    public async Task ShouldNotRequestPasswordUpdateWithEmptySession()
    {
        // Arrange
        _userSession.User.Returns((UserDto?)null);
        UpdatePasswordArgs args = new();

        // Act
        _sut.UpdatePasswordCommand.Execute(args);

        // Assert
        await _userService.DidNotReceive().LoginAsync(Arg.Any<string>(), Arg.Any<string>());
        await _userService.DidNotReceive().UpdateAsync(Arg.Any<User>());
        _hasher.DidNotReceive().HashPassword(Arg.Any<User>(), Arg.Any<string>());
    }

    [Fact]
    public async Task ShouldRequestAvatarUpdateWithCustomAvatar()
    {
        // Arrange
        _userSession.User.Returns(_dto);
        _userService.GetUserAsync(_dto.Id).Returns(new User(_dto.Id, _dto.Username, "pass", _dto.Avatar, false));
        BitmapImage img = new();
        _sut.Avatar = img;
        _userService.UpdateAsync(Arg.Any<User>()).Returns(Task.CompletedTask);

        // Act
        _sut.UpdateAvatarCommand.Execute(null);

        // Assert
        await _userService.Received().GetUserAsync(_dto.Id);
        _bitmapCreator.DidNotReceive().GetGenericAvatar(Arg.Any<string>());
        await _userService.Received().UpdateAsync(Arg.Is<User>(
            u => u.Id == _dto.Id
            && u.Username == _dto.Username
            && u.Password == "pass"
            && u.Avatar == img
            && u.HasCustomAvatar == true));
        _dto.Avatar.Should().Be(img);
        _sut.AvatarChangeSuccess.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ShouldRequestAvatarUpdateWithGenericAvatar()
    {
        // Arrange
        _userSession.User.Returns(_dto);
        _userService.GetUserAsync(_dto.Id).Returns(new User(_dto.Id, _dto.Username, "pass", _dto.Avatar, true));
        BitmapImage img = new();
        _bitmapCreator.GetGenericAvatar(_dto.Username).Returns(img);
        _sut.Avatar = null;
        _userService.UpdateAsync(Arg.Any<User>()).Returns(Task.CompletedTask);

        // Act
        _sut.UpdateAvatarCommand.Execute(null);

        // Assert
        await _userService.Received().GetUserAsync(_dto.Id);
        _bitmapCreator.Received().GetGenericAvatar(_dto.Username);
        await _userService.Received().UpdateAsync(Arg.Is<User>(
            u => u.Id == _dto.Id
            && u.Username == _dto.Username
            && u.Password == "pass"
            && u.Avatar == img
            && u.HasCustomAvatar == false));
        _dto.Avatar.Should().Be(img);
        _sut.AvatarChangeSuccess.Should().NotBeEmpty();
    }

    [Fact]
    public void ShouldOpenFileDialogue()
    {
        // Arrange
        BitmapImage img = new();
        _fileService.GetImageFile().Returns(img);

        // Act
        _sut.ChooseAvatarCommand.Execute(null);

        // Assert
        _fileService.Received().GetImageFile();
        _sut.Avatar.Should().Be(img);
    }
}
