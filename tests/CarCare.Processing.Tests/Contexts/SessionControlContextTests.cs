using CarCare.Processing.Contexts;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using FluentAssertions;
using NSubstitute;
using System.Reflection;
using System.Windows;

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
            .GetProperty(nameof(SessionControlContext.MenuVisibility))!;

        isOpenProperty.SetValue(_sut, Visibility.Visible);
    }

    [Fact]
    public void ShouldRequestUserLogoutAndNavigateToAuthenticationView()
    {
        Thread sta = new(() =>
        {
            // Arrange
            Window window = new();
            _userSession.LogoutUser();
            _navigationService.NavigateTo<IAuthenticationContext>(window);

            // Act
            _sut.LogoutCommand.Execute(window);

            // Assert window while in scope
            _navigationService.Received().NavigateTo<IAuthenticationContext>(window);
        });
        sta.SetApartmentState(ApartmentState.STA);
        sta.Start();
        sta.Join();

        // Assert
        _userSession.Received().LogoutUser();
    }

    [Fact]
    public void MenuShouldBeHiddenOnStart()
    {
        // Act
        Visibility result = _sut.MenuVisibility;

        // Assert
        result.Should().Be(Visibility.Collapsed);
    }

    [Fact]
    public void ToggleShouldShowMenu()
    {
        // Act
        _sut.ToggleMenuCommand.Execute(null);

        // Assert
        _sut.MenuVisibility.Should().Be(Visibility.Visible);
    }

    [Fact]
    public void ToggleShouldHideMenu()
    {
        // Arrange
        ForceShowMenu();

        // Act
        _sut.ToggleMenuCommand.Execute(null);

        // Assert
        _sut.MenuVisibility.Should().Be(Visibility.Collapsed);
    }

    [Fact]
    public void CloseShouldLeaveMenuHidden()
    {
        // Act
        _sut.CloseMenuCommand.Execute(null);

        // Assert
        _sut.MenuVisibility.Should().Be(Visibility.Collapsed);
    }

    [Fact]
    public void CloseShowHideMenu()
    {
        // Arrange
        ForceShowMenu();

        // Act
        _sut.CloseMenuCommand.Execute(null);

        // Assert
        _sut.MenuVisibility.Should().Be(Visibility.Collapsed);
    }
}
