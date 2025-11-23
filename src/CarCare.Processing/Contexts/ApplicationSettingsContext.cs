using CarCare.Processing.Abstract;
using CarCare.Processing.Commands;
using CarCare.Processing.Enums;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models.Core;
using System.Windows.Input;

namespace CarCare.Processing.Contexts;

internal class ApplicationSettingsContext : Context, IApplicationSettingsContext
{
    private readonly IThemeService _themeService;
    private readonly IUserSettingsService _userSettingsService;
    private readonly IUserSession _session;

    public ApplicationTheme[] AvailableThemes => Enum.GetValues<ApplicationTheme>();

    public Currency[] AvailableCurrencies => Enum.GetValues<Currency>();

    public ApplicationTheme Theme
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public Currency PreferredCurrency
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public ICommand UpdateThemeCommand { get; }

    public ICommand UpdatePreferredCurrencyCommand { get; }

    public ApplicationSettingsContext(
        IThemeService themeService,
        IUserSettingsService userSettingsService,
        IUserSession session)
    {
        _themeService = themeService;
        _userSettingsService = userSettingsService;
        _session = session;
        Theme = _themeService.Theme;
        UpdateThemeCommand = new AsyncCommand(UpdateTheme);
        UpdatePreferredCurrencyCommand = new AsyncCommand(UpdatePreferredCurrency);
    }

    private async Task UpdateTheme()
    {
        ApplicationTheme theme = Theme;
        _themeService.ChangeTheme(theme);
        Guid id = _session.User!.Id;
        UserSettings settings = await _userSettingsService.GetAsync(id);
        settings.Theme = theme;
        await _userSettingsService.UpdateAsync(settings);
    }

    private async Task UpdatePreferredCurrency()
    {
        Currency currency = PreferredCurrency;
        Guid id = _session.User!.Id;
        UserSettings settings = await _userSettingsService.GetAsync(id);
        settings.PreferredCurrency = currency;
        await _userSettingsService.UpdateAsync(settings);
    }
}
