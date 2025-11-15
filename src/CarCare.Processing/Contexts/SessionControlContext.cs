using CarCare.Processing.Abstract;
using CarCare.Processing.Commands;
using CarCare.Processing.Enums;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models.Dto;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace CarCare.Processing.Contexts;

internal class SessionControlContext : Context, ISessionControlContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationService _navigationService;
    private readonly IUserSession _userSession;
    private readonly IThemeService _themeService;

    public Visibility MenuVisibility 
    {
        get => field; 
        private set
        {
            field = value;
            OnPropertyChanged();
        } 
    }

    public UserDto User { get; }

    public ApplicationTheme Theme
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
            UpdateTheme();
        }
    }

    public ICommand LogoutCommand { get; }

    public ICommand ToggleMenuCommand { get; }

    public ICommand CloseMenuCommand { get; }

    public ICommand ShowAccountSettingsCommand { get; }

    public SessionControlContext(
        IServiceProvider serviceProvider,
        INavigationService navigationService, 
        IUserSession userSession, 
        IThemeService themeService)
    {
        _serviceProvider = serviceProvider;
        _navigationService = navigationService;
        _userSession = userSession;
        _themeService = themeService;
        User = _userSession.User!;
        MenuVisibility = Visibility.Collapsed;
        LogoutCommand = new Command(Logout);
        ToggleMenuCommand = new Command(ToggleMenu);
        CloseMenuCommand = new Command(CloseMenu);
        ShowAccountSettingsCommand = new Command(ShowAccountSettings);
    }

    private void Logout(object? parameter)
    {
        if (parameter is not Window window)
        {
            return;
        }

        _userSession.LogoutUser();
        _navigationService.NavigateTo<IAuthenticationContext>(window);
        _themeService.SetDefaultTheme();
    }

    private void ToggleMenu(object? parameter)
    {
        MenuVisibility = MenuVisibility switch
        {
            Visibility.Collapsed => Visibility.Visible,
            Visibility.Visible => Visibility.Collapsed,
            _ => throw new UnreachableException()
        };
    }

    private void CloseMenu(object? parameter)
    {
        MenuVisibility = Visibility.Collapsed;
    }

    private void UpdateTheme()
    {
        _themeService.ChangeTheme(Theme);
    }

    private void ShowAccountSettings(object? parameter)
    {
        _navigationService.NavigateTo<ISettingsContext>();
        ISettingsContext settingsContext = _serviceProvider.GetRequiredService<ISettingsContext>();
        settingsContext.Tab = SettingsTab.AccountSettings;
        CloseMenu(null);
    }
}
