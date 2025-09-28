using CarCare.Persistence.Models;

namespace CarCare.Persistence.Interfaces;

/// <summary>
/// A repository for performing CRUD operations with the
/// Users table.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Adds a new <see cref="UserDao"/> entry.
    /// </summary>
    /// <param name="user">The <see cref="UserDao"/> to register.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task AddAsync(UserDao user);

    /// <summary>
    /// Updates an existing <see cref="UserDao"/> entry to the new values.
    /// </summary>
    /// <param name="user">A <see cref="UserDao"/> with the desired values.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task UpdateAsync(UserDao user);

    /// <summary>
    /// Deletes the requested user by <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The <see cref="Guid"/> of the user.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Deletes the requested user by <paramref name="username"/>.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task DeleteAsync(string username);
    
    /// <summary>
    /// Returns the requested user by <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The id of the desired user.</param>
    /// <returns>A matching <see cref="UserDao"/>.</returns>
    Task<UserDao> GetUserAsync(Guid id);

    /// <summary>
    /// Returns the requested user by <paramref name="username"/>.
    /// </summary>
    /// <param name="username">The username of the desired user.</param>
    /// <returns>A matching <see cref="UserDao"/>.</returns>
    Task<UserDao> GetUserAsync(string username);

    /// <summary>
    /// Returns all <see cref="UserDao"/> entries.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="UserDao"/>.</returns>
    Task<IEnumerable<UserDao>> GetUsersAsync();
}
