using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CarCare.Processing.Interfaces.Context;

/// <summary>
/// Data context for the Account Setting user control.
/// </summary>
public interface IAccountSettingsContext
{
    /// <summary>
    /// The value of the username text box for username updating.
    /// </summary>
    string Username { get; set; }

    /// <summary>
    /// An error that occurred during username update.
    /// </summary>
    public string UsernameChangeError { get; }

    /// <summary>
    /// An error that occurred during password update.
    /// </summary>
    public string PasswordChangeError { get; }

    /// <summary>
    /// Informs the user that the username was updated.
    /// </summary>
    public string UsernameChangeSuccess { get; }

    /// <summary>
    /// Informs the user that the password was updated.
    /// </summary>
    public string PasswordChangeSuccess { get; }

    /// <summary>
    /// Informs the user that the avatar was updated.
    /// </summary>
    public string AvatarChangeSuccess { get; }

    /// <summary>
    /// The user's avatar.
    /// </summary>
    public BitmapImage? Avatar { get; }

    /// <summary>
    /// Sets a new username for the logged-in user.
    /// </summary>
    ICommand UpdateUsernameCommand { get; }

    /// <summary>
    /// Sets a new password for the logged-in user.
    /// </summary>
    ICommand UpdatePasswordCommand { get; }

    /// <summary>
    /// Sets a new avatar image for the logged-in user.
    /// </summary>
    ICommand UpdateAvatarCommand { get; }

    /// <summary>
    /// Opens a dialogue to select a new avatar.
    /// </summary>
    ICommand ChooseAvatarCommand { get; }

    /// <summary>
    /// Deletes the custom avatar.
    /// </summary>
    ICommand DeleteAvatarCommand { get; }
}
