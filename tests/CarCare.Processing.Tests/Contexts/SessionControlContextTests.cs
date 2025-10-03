using CarCare.Processing.Contexts;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using NSubstitute;

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
}
