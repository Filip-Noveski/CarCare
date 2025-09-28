using CarCare.Persistence.Models;

namespace CarCare.Persistence.Interfaces;

/// <summary>
/// A repository for performing CRUD operations with the
/// MigrationsHistory table.
/// </summary>
public interface IMigrationsHistoryRepository
{
    /// <summary>
    /// Checks whether the MigrationsHistory table exists.
    /// </summary>
    /// <returns>A <see cref="bool"/> representing whether the table exists.</returns>
    Task<bool> ExistsAsync();

    /// <summary>
    /// Adds a new <see cref="MigrationDao"/> entry to the database.
    /// </summary>
    /// <param name="migration">The migration that has been applied.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task AddAsync(MigrationDao migration);

    /// <summary>
    /// Returns the laterst applied <see cref="MigrationDao"/>.
    /// </summary>
    /// <returns>The latest applied <see cref="MigrationDao"/>.</returns>
    Task<MigrationDao> GetLatestMigrationAsync();
}
