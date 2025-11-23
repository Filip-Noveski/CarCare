using CarCare.Processing.Enums;
using System.Windows.Input;

namespace CarCare.Processing.Interfaces.Context;

/// <summary>
/// Data context for the application settings view component.
/// </summary>
public interface IApplicationSettingsContext
{
    /// <summary>
    /// The themes that the user can choose from.
    /// </summary>
    ApplicationTheme[] AvailableThemes { get; }

    /// <summary>
    /// The currencies that the user can see data in.
    /// </summary>
    Currency[] AvailableCurrencies { get; }

    /// <summary>
    /// The theme of the application.
    /// </summary>
    ApplicationTheme Theme { get; set; }

    /// <summary>
    /// The preferred currency by the user.
    /// </summary>
    Currency PreferredCurrency { get; set; }

    /// <summary>
    /// Updates the theme.
    /// </summary>
    ICommand UpdateThemeCommand { get; }

    /// <summary>
    /// Updates the preferred currency.
    /// </summary>
    ICommand UpdatePreferredCurrencyCommand { get; }
}
