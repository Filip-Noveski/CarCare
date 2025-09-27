using CarCare.Persistence.Models;

namespace CarCare.Persistence.Interfaces;

/// <summary>
/// A repository for performing CRUD operations with the
/// Users table.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Adds a new <see cref="User"/> entry.
    /// </summary>
    /// <param name="user">The <see cref="User"/> to register.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task AddAsync(User user);

    /// <summary>
    /// Updates an existing <see cref="User"/> entry to the new values.
    /// </summary>
    /// <param name="user">A <see cref="User"/> with the desired values.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task UpdateAsync(User user);

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
    /// <returns>A matching <see cref="User"/>.</returns>
    Task<User> GetUserAsync(Guid id);

    /// <summary>
    /// Returns the requested user by <paramref name="username"/>.
    /// </summary>
    /// <param name="username">The username of the desired user.</param>
    /// <returns>A matching <see cref="User"/>.</returns>
    Task<User> GetUserAsync(string username);

    /// <summary>
    /// Returns all <see cref="User"/> entries.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="User"/>.</returns>
    Task<IEnumerable<User>> GetUsersAsync();
}
