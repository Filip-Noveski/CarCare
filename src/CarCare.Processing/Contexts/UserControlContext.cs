using CarCare.Processing.Commands;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using System.Windows.Input;

namespace CarCare.Processing.Contexts;

internal class UserControlContext : IUserControlContext
{
    private readonly INavigationService _navigationService;
    private readonly IUserSession _userSession;

    public Guid UserId { get; }

    public ICommand LogoutCommand { get; }

    public UserControlContext(INavigationService navigationService, IUserSession userSession)
    {
        _navigationService = navigationService;
        _userSession = userSession;
        LogoutCommand = new Command(Logout);
    }

    private void Logout(object? parameter)
    {
        _userSession.LogoutUser();
        _navigationService.NavigateTo<IAuthenticationContext>();
    }
}
