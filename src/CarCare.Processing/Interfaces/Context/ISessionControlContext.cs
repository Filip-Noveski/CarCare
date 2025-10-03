using CarCare.Processing.Models;
using System.Windows.Input;

namespace CarCare.Processing.Interfaces.Context;

/// <summary>
/// A data context for the user controls.
/// </summary>
public interface ISessionControlContext
{
    /// <summary>
    /// The logged-in user.
    /// </summary>
    UserDto User { get; }

    /// <summary>
    /// A command that logs the user out.
    /// </summary>
    ICommand LogoutCommand { get; }
}
