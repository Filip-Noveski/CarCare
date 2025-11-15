using CarCare.Processing.Enums;

namespace CarCare.Processing.Interfaces.Context;

/// <summary>
/// Data context for the Account Settings window.
/// </summary>
public interface ISettingsContext : IWindowContext
{
    /// <summary>
    /// The selected settings tab.
    /// </summary>
    SettingsTab Tab { get; set; }
}
