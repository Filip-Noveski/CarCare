using System.Windows.Input;

namespace CarCare.Processing.Interfaces.Context;

/// <summary>
/// A data context for the user controls.
/// </summary>
public interface IUserControlContext
{
    /// <summary>
    /// The id of the logged-in user.
    /// </summary>
    Guid UserId { get; }

    /// <summary>
    /// A command that logs the user out.
    /// </summary>
    ICommand LogoutCommand { get; }
}
