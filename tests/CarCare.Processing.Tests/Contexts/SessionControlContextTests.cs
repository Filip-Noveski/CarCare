using CarCare.Processing.Contexts;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using FluentAssertions;
using NSubstitute;
using System.Reflection;

namespace CarCare.Processing.Tests.Contexts;

public class SessionControlContextTests
{
    private readonly INavigationService _navigationService;
    private readonly IUserSession _userSession;
    private readonly SessionControlContext _sut;

    public SessionControlContextTests()
    {
        _navigationService = Substitute.For<INavigationService>();
        _userSession = Substitute.For<IUserSession>();
        _sut = new(_navigationService, _userSession);
    }

    private void ForceShowMenu()
    {
        PropertyInfo isOpenProperty = typeof(SessionControlContext)
            .GetProperty(nameof(SessionControlContext.IsOpen))!;

        isOpenProperty.SetValue(_sut, true);
    }

    [Fact]
    public void ShouldRequestUserLogoutAndNavigateToAuthenticationView()
    {
        // Arrange
        _userSession.LogoutUser();
        _navigationService.NavigateTo<IAuthenticationContext>();

        // Act
        _sut.LogoutCommand.Execute(null);

        // Assert
        _userSession.Received().LogoutUser();
        _navigationService.Received().NavigateTo<IAuthenticationContext>();
    }

    [Fact]
    public void MenuShouldBeHiddenOnStart()
    {
        // Act
        bool result = _sut.IsOpen;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ToggleShouldShowMenu()
    {
        // Act
        _sut.ToggleMenuCommand.Execute(null);

        // Assert
        _sut.IsOpen.Should().BeTrue();
    }

    [Fact]
    public void ToggleShouldHideMenu()
    {
        // Arrange
        ForceShowMenu();

        // Act
        _sut.ToggleMenuCommand.Execute(null);

        // Assert
        _sut.IsOpen.Should().BeFalse();
    }

    [Fact]
    public void CloseShouldLeaveMenuHidden()
    {
        // Act
        _sut.CloseMenuCommand.Execute(null);

        // Assert
        _sut.IsOpen.Should().BeFalse();
    }

    [Fact]
    public void CloseShowHideMenu()
    {
        // Arrange
        ForceShowMenu();

        // Act
        _sut.CloseMenuCommand.Execute(null);

        // Assert
        _sut.IsOpen.Should().BeFalse();
    }
}
