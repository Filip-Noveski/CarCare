using System.Windows.Input;

namespace CarCare.Processing.Interfaces.Context;

/// <summary>
/// A data context for windows.
/// </summary>
public interface IWindowContext
{
    /// <summary>
    /// The character to show on the Toggle Maximise Button.
    /// </summary>
    string MaximiseButtonChar { get; }

    /// <summary>
    /// Closes the window.
    /// </summary>
    ICommand CloseCommand { get; }

    /// <summary>
    /// Toggles between fullscreen and windowed modes.
    /// </summary>
    ICommand ToggleMaximiseCommand { get; }

    /// <summary>
    /// Minimises the window.
    /// </summary>
    ICommand MinimiseCommand { get; }
}
