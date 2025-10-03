using CarCare.Processing.Commands;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models;
using System.Windows.Input;

namespace CarCare.Processing.Contexts;

internal class SessionControlContext : ISessionControlContext
{
    private readonly INavigationService _navigationService;
    private readonly IUserSession _userSession;

    public UserDto User { get; }

    public ICommand LogoutCommand { get; }

    public SessionControlContext(INavigationService navigationService, IUserSession userSession)
    {
        _navigationService = navigationService;
        _userSession = userSession;
        User = _userSession.User!;
        LogoutCommand = new Command(Logout);
    }

    private void Logout(object? parameter)
    {
        _userSession.LogoutUser();
        _navigationService.NavigateTo<IAuthenticationContext>();
    }
}
