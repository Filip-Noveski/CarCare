using System.Windows.Input;

namespace CarCare.Processing.Interfaces.Context;

/// <summary>
/// A data context for the Login control.
/// </summary>
public interface ILoginContext
{
    /// <summary>
    /// The command to log the user in.
    /// </summary>
    ICommand LoginCommand { get; }

    /// <summary>
    /// The username of the user.
    /// </summary>
    string Username { get; }

    /// <summary>
    /// An error that may have occurred during login.
    /// </summary>
    string Error { get; }
}
