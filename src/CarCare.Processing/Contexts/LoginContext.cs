using CarCare.Processing.Abstract;
using CarCare.Processing.Commands;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Models;
using OneOf;
using System.Windows.Input;

namespace CarCare.Processing.Contexts;

internal class LoginContext : Context, ILoginContext
{
    private readonly IUserService _userService;
    private readonly INavigationService _navigationService;

    public ICommand LoginCommand { get; }

    public string Username
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public string Error
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public LoginContext(IUserService userService, INavigationService navigationService)
    {
        _userService = userService;
        _navigationService = navigationService;
        Username = string.Empty;
        Error = string.Empty;
        LoginCommand = new Command(Login);
    }

    private async void Login(object? parameter)
    {
        if (parameter is not AuthenticationArgs args)
        {
            Error = "An internal error occurred.";
            return;
        }

        string password = args.PasswordBox.Password;
        OneOf<User, LoginError> result = await _userService.LoginAsync(Username, password);
        result.Switch(
            user => _navigationService.NavigateTo<IDashboardContext>(args.Window),
            error => Error = error.Message);
    }
}
