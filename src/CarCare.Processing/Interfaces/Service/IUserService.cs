using CarCare.Processing.Models;
using OneOf;

namespace CarCare.Processing.Interfaces.Service;

/// <summary>
/// A service for managing <see cref="User"/> objects.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Creates a new <see cref="User"/>.
    /// </summary>
    /// <param name="user">The <see cref="User"/> to create.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task RegisterAsync(User user);

    /// <summary>
    /// Deletes the specified <see cref="User"/>.
    /// </summary>
    /// <param name="id">The id to delete by.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Deletes the specified <see cref="User"/>.
    /// </summary>
    /// <param name="username">The username to delete by.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task DeleteAsync(string username);

    /// <summary>
    /// Updates the <see cref="User"/> to contain the new values.
    /// </summary>
    /// <param name="user">The <see cref="User"/> with new parameters.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task UpdateAsync(User user);

    /// <summary>
    /// Tries to log the user in based on the credentials.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="password">The plaintext password.</param>
    /// <returns>
    /// The logged in <see cref="User"/> - if login was successful;
    /// A <see cref="LoginError"/> - if login failed.
    /// </returns>
    Task<OneOf<User, LoginError>> LoginAsync(string username, string password);

    /// <summary>
    /// Returns the specified <see cref="User"/>.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <returns>The discovered <see cref="User"/>.</returns>
    Task<User> GetUserAsync(Guid id);

    /// <summary>
    /// Returns the specified <see cref="User"/>.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>The discovered <see cref="User"/>.</returns>
    Task<User> GetUserAsync(string username);

    /// <summary>
    /// Returns all users in the database.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="User"/>.</returns>
    Task<IEnumerable<User>> GetUsersAsync();
}
