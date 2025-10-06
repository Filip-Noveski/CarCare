using CarCare.Processing.Abstract;
using CarCare.Processing.Commands;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Models.Communication;
using CarCare.Processing.Models.Core;
using Microsoft.AspNetCore.Identity;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CarCare.Processing.Contexts;

internal class RegisterContext : Context, IRegisterContext
{
    private readonly IUserService _userService;
    private readonly IPasswordHasher<User> _hasher;
    private readonly INavigationService _navigationService;
    private readonly IBitmapCreatorService _bitmapService;
    private readonly IAuthenticationContext _authenticationContext;
    private readonly IFileDialogueService _fileService;

    public ICommand RegisterCommand { get; }

    public ICommand ChooseAvatarCommand { get; }

    public ICommand NavigateToLoginCommand { get; }

    public BitmapImage? Avatar
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

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
        INavigationService navigationService,
        IBitmapCreatorService bitmapService,
        IAuthenticationContext authenticationContext,
        IFileDialogueService fileService)
    {
        _userService = userService;
        _hasher = hasher;
        _navigationService = navigationService;
        _bitmapService = bitmapService;
        _authenticationContext = authenticationContext;
        _fileService = fileService;
        Username = string.Empty;
        Error = string.Empty;
        RegisterCommand = new Command(Register);
        ChooseAvatarCommand = new Command(ChooseAvatar);
        NavigateToLoginCommand = new Command(NavigateToLogin);
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

        // parameter check success
        User user = (Avatar is null) switch
        {
            true => User.Create(Username, password, _hasher, _bitmapService),
            false => User.Create(Username, password, Avatar, _hasher)
        };
        await _userService.RegisterAsync(user);

        _navigationService.NavigateTo<IDashboardContext>(args.Window);
    }

    private void ChooseAvatar(object? parameter)
    {
        Avatar = _fileService.GetImageFile();
    }

    private void NavigateToLogin(object? parameter)
    {
        _authenticationContext.OnLoginRequested();
    }
}
