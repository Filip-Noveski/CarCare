using CarCare.Processing.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CarCare.Processing.Interfaces.Context;

/// <summary>
/// A data context for the Login control.
/// </summary>
public interface ILoginContext
{
    /// <summary>
    /// An event where the username's value changed.
    /// </summary>
    event EventHandler UsernameChanged;

    /// <summary>
    /// The command to log the user in.
    /// </summary>
    ICommand LoginCommand { get; }

    /// <summary>
    /// The command to navigate to the Register page.
    /// </summary>
    ICommand RegisterCommand { get; }

    /// <summary>
    /// The command that selects a user.
    /// </summary>
    ICommand SelectUserCommand { get; }

    /// <summary>
    /// The command that returns the view to the users list.
    /// </summary>
    ICommand ReturnToUsersListCommand { get; }

    /// <summary>
    /// A list of all registered users.
    /// </summary>
    ObservableCollection<UserDto> Users { get; } 

    /// <summary>
    /// The username of the selected user.
    /// </summary>
    string Username { get; }

    /// <summary>
    /// The avatar image of the selected user.
    /// </summary>
    BitmapImage? Avatar { get; }

    /// <summary>
    /// An error that may have occurred during login.
    /// </summary>
    string Error { get; }

    /// <summary>
    /// Initialises the data context.
    /// </summary>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task InitialiseAsync();

}
