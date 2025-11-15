using System.Windows.Input;

namespace CarCare.Processing.Interfaces.Context;

/// <summary>
/// A data context for the main Dashboard window of the application.
/// </summary>
public interface IDashboardContext : IWindowContext
{
    /// <summary>
    /// Opens the settings window with the App Settings tab displayed.
    /// </summary>
    ICommand OpenAppSettingsCommand { get; }
}
