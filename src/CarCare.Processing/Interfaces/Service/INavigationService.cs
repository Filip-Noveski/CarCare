using System.Windows;

namespace CarCare.Processing.Interfaces.Service;

/// <summary>
/// Provides methods to switch windows.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Navigates to the window of the specified data context.
    /// </summary>
    /// <typeparam name="TContext">The type of data context bound to the desired window.</typeparam>
    void NavigateTo<TContext>();

    /// <summary>
    /// Navigates to the window of the specified data context whilst closing
    /// the previously open <see cref="Window"/>.
    /// </summary>
    /// <typeparam name="TContext">The type of data context bound to the desired window.</typeparam>
    /// <param name="window">The <see cref="Window"/> to close.</param>
    void NavigateTo<TContext>(Window window);
}
