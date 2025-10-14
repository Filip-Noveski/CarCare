using CarCare.Processing.Enums;

namespace CarCare.Processing.Interfaces.Service;

/// <summary>
/// A service for managing application themes.
/// </summary>
public interface IThemeService
{
    /// <summary>
    /// The currently selected <see cref="ApplicationTheme"/>.
    /// </summary>
    ApplicationTheme Theme { get; }

    /// <summary>
    /// Changes the application theme to the provided <paramref name="theme"/>.
    /// </summary>
    /// <param name="theme">The <see cref="ApplicationTheme"/> to set.</param>
    void ChangeTheme(ApplicationTheme theme);

    /// <summary>
    /// Sets the initial 
    /// </summary>
    void SetDefaultTheme();
}
