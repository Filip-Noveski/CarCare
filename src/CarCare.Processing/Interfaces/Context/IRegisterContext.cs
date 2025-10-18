using System.Windows.Input;
using System.Windows.Media.Imaging;

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
    /// The command to choose a custom user avatar.
    /// </summary>
    ICommand ChooseAvatarCommand { get; }

    /// <summary>
    /// The command to navigate to the Login view.
    /// </summary>
    ICommand NavigateToLoginCommand { get; }

    /// <summary>
    /// The selected avatar image.
    /// </summary>
    BitmapImage? Avatar { get; }

    /// <summary>
    /// The username of the user.
    /// </summary>
    string Username { get; }

    /// <summary>
    /// An error that may have occurred during registration.
    /// </summary>
    string Error { get; }
}
