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
    /// The theme of the application.
    /// </summary>
    ApplicationTheme Theme { get; set; }

    /// <summary>
    /// Updates the theme.
    /// </summary>
    ICommand UpdateThemeCommand { get; }
}
