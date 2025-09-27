namespace CarCare.Persistence.Interfaces;

/// <summary>
/// Provides methods for managing database creation and updating.
/// </summary>
public interface IDatabaseManager
{
    /// <summary>
    /// Checks whether the database exists and is up to date; creates or updates
    /// it if necessary.
    /// </summary>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    public Task VerifyDatabaseStateAsync();
}
