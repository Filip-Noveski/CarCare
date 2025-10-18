using CarCare.Processing.Enums;
using CarCare.Processing.Models.Dto;
using System.Windows;
using System.Windows.Input;

namespace CarCare.Processing.Interfaces.Context;

/// <summary>
/// A data context for the user controls.
/// </summary>
public interface ISessionControlContext
{
    /// <summary>
    /// Whether the session control menu is visible.
    /// </summary>
    Visibility MenuVisibility { get; }

    /// <summary>
    /// The logged-in user.
    /// </summary>
    UserDto User { get; }

    /// <summary>
    /// The selected application theme.
    /// </summary>
    ApplicationTheme Theme { get; set; }

    /// <summary>
    /// A command that logs the user out.
    /// </summary>
    ICommand LogoutCommand { get; }

    /// <summary>
    /// Toggles the menu between open and closed.
    /// </summary>
    ICommand ToggleMenuCommand { get; }

    /// <summary>
    /// Hides the menu.
    /// </summary>
    ICommand CloseMenuCommand { get; }
}
