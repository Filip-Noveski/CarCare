using System.Windows;
using System.Windows.Controls;

namespace CarCare.Processing.Models;

/// <summary>
/// A model containing the arguments for user registration.
/// </summary>
public class AuthenticationArgs
{
    /// <summary>
    /// The authentication window (register or login) to close on success.
    /// </summary>
    public Window Window { get; set; } = null!;

    /// <summary>
    /// The password box containing the password to read.
    /// </summary>
    public PasswordBox PasswordBox { get; set; } = null!;
}
