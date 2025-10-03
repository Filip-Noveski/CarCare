using CarCare.Processing.Abstract;
using CarCare.Processing.Commands;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models;
using System.Windows.Input;

namespace CarCare.Processing.Contexts;

internal class SessionControlContext : Context, ISessionControlContext
{
    private readonly INavigationService _navigationService;
    private readonly IUserSession _userSession;

    public bool IsOpen 
    {
        get => field; 
        private set
        {
            field = value;
            OnPropertyChanged();
        } 
    }

    public UserDto User { get; }

    public ICommand LogoutCommand { get; }

    public ICommand ToggleMenuCommand { get; }

    public ICommand CloseMenuCommand { get; }

    public SessionControlContext(INavigationService navigationService, IUserSession userSession)
    {
        _navigationService = navigationService;
        _userSession = userSession;
        User = _userSession.User!;
        IsOpen = false;
        LogoutCommand = new Command(Logout);
        ToggleMenuCommand = new Command(ToggleMenu);
        CloseMenuCommand = new Command(CloseMenu);
    }

    private void Logout(object? parameter)
    {
        _userSession.LogoutUser();
        _navigationService.NavigateTo<IAuthenticationContext>();
    }

    private void ToggleMenu(object? parameter)
    {
        IsOpen = !IsOpen;
    }

    private void CloseMenu(object? parameter)
    {
        IsOpen = false;
    }
}
