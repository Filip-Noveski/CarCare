using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.UserInterface.Windows;
using System.Windows;

namespace CarCare.UserInterface.Services;

internal class NavigationService : INavigationService
{
    private readonly Dictionary<Type, Type> _views = new()
    {
        { typeof(IDashboardContext), typeof(Dashboard) },
        { typeof(ISplashScreenContext), typeof(Splash) },
        { typeof(IAuthenticationContext), typeof(Authentication) },
        { typeof(IAccountSettingsContext), typeof(AccountSettings) }
    };
    private readonly IServiceProvider _provider;

    public NavigationService(IServiceProvider provider)
    {
        _provider = provider;
    }

    public void NavigateTo<TContext>()
    {
        Window window = (Window)_provider.GetService(_views[typeof(TContext)])!;
        window.Show();
    }

    public void NavigateTo<TContext>(Window window)
    {
        NavigateTo<TContext>();
        window.Close();
    }
}
