using System.Windows.Input;

namespace CarCare.Processing.Interfaces.Context;

/// <summary>
/// A data context for the Register control.
/// </summary>
public interface IRegisterContext
{
    /// <summary>
    /// The command to register a new user.
    /// </summary>
    ICommand RegisterCommand { get; }

    /// <summary>
    /// The username of the user.
    /// </summary>
    string Username { get; }

    /// <summary>
    /// An error that may have occurred during registration.
    /// </summary>
    string Error { get; }
}
