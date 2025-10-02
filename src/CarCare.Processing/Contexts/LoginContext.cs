using CarCare.Processing.Abstract;
using CarCare.Processing.Commands;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Models;
using OneOf;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CarCare.Processing.Contexts;

internal class LoginContext : Context, ILoginContext
{
    private readonly IUserService _userService;
    private readonly INavigationService _navigationService;
    private readonly IAuthenticationContext _authenticationContext;

    public event EventHandler UsernameChanged = null!;

    public ICommand LoginCommand { get; }

    public ICommand RegisterCommand { get; }

    public ICommand SelectUserCommand { get; }

    public ICommand ReturnToUsersListCommand { get; }

    public ObservableCollection<UserDto> Users { get; private set; }

    public string Username
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
            OnUsernameChanged();
        }
    }

    public BitmapImage? Avatar
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

    public LoginContext(
        IUserService userService,
        INavigationService navigationService,
        IAuthenticationContext authenticationContext)
    {
        _userService = userService;
        _navigationService = navigationService;
        _authenticationContext = authenticationContext;
        Users = new();
        Username = string.Empty;
        Avatar = null;
        Error = string.Empty;
        LoginCommand = new Command(Login);
        RegisterCommand = new Command(NavigateToRegister);
        SelectUserCommand = new Command(SelectUser);
        ReturnToUsersListCommand = new Command(ReturnToUsersList);
    }

    public async Task InitialiseAsync()
    {
        IEnumerable<UserDto> users = (await _userService.GetUsersAsync())
            .Select(x => new UserDto(x.Id, x.Username, x.Avatar));
        Users.Clear();
        foreach (UserDto user in users)
        {
            Users.Add(user);
        }

        if (Users.Count is 0)
        {
            _authenticationContext.OnRegisterRequested();
        }
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

    private void NavigateToRegister(object? parameter)
    {
        _authenticationContext.OnRegisterRequested();
    }

    private void SelectUser(object? parameter)
    {
        if (parameter is not string username)
        {
            Error = "An internal error occurred.";
            return;
        }

        UserDto user = Users.First(x => x.Username == username);
        Username = username;
        Avatar = user.Avatar;
    }

    private void ReturnToUsersList(object? parameter)
    {
        Username = string.Empty;
        Avatar = null;
    }

    private void OnUsernameChanged()
    {
        UsernameChanged?.Invoke(this, EventArgs.Empty);
    }
}
