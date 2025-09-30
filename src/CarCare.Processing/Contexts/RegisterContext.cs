using CarCare.Processing.Abstract;
using CarCare.Processing.Commands;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Models;
using Microsoft.AspNetCore.Identity;
using System.Windows.Input;

namespace CarCare.Processing.Contexts;

internal class RegisterContext : Context, IRegisterContext
{
    private readonly IUserService _userService;
    private readonly IPasswordHasher<User> _hasher;
    private readonly INavigationService _navigationService;

    public ICommand RegisterCommand { get; }

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

    public RegisterContext(
        IUserService userService, 
        IPasswordHasher<User> hasher, 
        INavigationService navigationService)
    {
        _userService = userService;
        _hasher = hasher;
        _navigationService = navigationService;
        Username = string.Empty;
        Error = string.Empty;
        RegisterCommand = new Command(Register);
    }

    private async void Register(object? parameter)
    {
        if (parameter is not AuthenticationArgs args)
        {
            Error = "An internal error occurred.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Username))
        {
            Error = "The username cannot be empty or whitespace";
            return;
        }

        string password = args.PasswordBox.Password;
        if (string.IsNullOrWhiteSpace(password))
        {
            Error = "The password cannot be empty or whitespace.";
            return;
        }

        // login success
        User user = User.Create(Username, password, _hasher);
        await _userService.RegisterAsync(user);

        _navigationService.NavigateTo<IDashboardContext>(args.Window);
    }
}
