using CarCare.Persistence.Models;

namespace CarCare.Persistence.Interfaces;

/// <summary>
/// A repository for performing CRUD operations with the
/// CarLifecycles table.
/// </summary>
public interface ICarLifecycleRepository
{
    /// <summary>
    /// Adds a new <paramref name="carLifecycle"/> entry.
    /// </summary>
    /// <param name="carLifecycle">The <see cref="CarLifecycleDao"/> to add.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task AddAsync(CarLifecycleDao carLifecycle);

    /// <summary>
    /// Deletes the car lifecycle with the provided <paramref name="carId"/>.
    /// </summary>
    /// <param name="carId">The car id to delete by.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task DeleteAsync(Guid carId);

    /// <summary>
    /// Gets the <see cref="CarLifecycleDao"/> by the <paramref name="carId"/>.
    /// </summary>
    /// <param name="carId">The car id to search by.</param>
    /// <returns>The requested <see cref="CarLifecycleDao"/>.</returns>
    Task<CarLifecycleDao> GetAsync(Guid carId);

    /// <summary>
    /// Updates the provided <see cref="CarLifecycleDao"/> entry.
    /// </summary>
    /// <param name="carLifecycle">The <see cref="CarLifecycleDao"/> with the new data.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task UpdateAsync(CarLifecycleDao carLifecycle);
}
