using CarCare.Persistence.Models;

namespace CarCare.Persistence.Interfaces;

/// <summary>
/// A repository for performing CRUD operations with the
/// UserSettings table.
/// </summary>
public interface IUserSettingsRepository
{
    /// <summary>
    /// Adds a new entry in the settings.
    /// </summary>
    /// <param name="settings">The <see cref="UserSettingsDao"/> to add.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task AddAsync(UserSettingsDao settings);

    /// <summary>
    /// Updates the required user settings entry.
    /// </summary>
    /// <param name="settings">The <see cref="UserSettingsDao"/> to update.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task UpdateAsync(UserSettingsDao settings);

    /// <summary>
    /// Deletes the settings entry with the specified <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">The <see cref="Guid"/> to delete by.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task DeleteAsync(Guid userId);

    /// <summary>
    /// Gets the user settings of the user with the specified <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">The <see cref="Guid"/> to search by.</param>
    /// <returns>The discovered <see cref="UserSettingsDao"/>.</returns>
    Task<UserSettingsDao> GetAsync(Guid userId);
}
