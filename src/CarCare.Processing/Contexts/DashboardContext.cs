using CarCare.Processing.Commands;
using CarCare.Processing.Enums;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace CarCare.Processing.Contexts;

internal class DashboardContext : WindowContext, IDashboardContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationService _navigationService;

    public ICommand OpenAppSettingsCommand { get; }

    public DashboardContext(IServiceProvider serviceProvider, INavigationService navigationService)
    {
        _serviceProvider = serviceProvider;
        _navigationService = navigationService;
        OpenAppSettingsCommand = new Command(OpenAppSettings);
    }

    private void OpenAppSettings(object? parameter)
    {
        _navigationService.NavigateTo<ISettingsContext>();
        ISettingsContext settingsContext = _serviceProvider.GetRequiredService<ISettingsContext>();
        settingsContext.Tab = SettingsTab.ApplicationSettings;
    }
}
