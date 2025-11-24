using CarCare.Persistence.Models;

namespace CarCare.Persistence.Interfaces;

/// <summary>
/// A repository for performing CRUD operations with the
/// Cars table.
/// </summary>
public interface ICarRepository
{
    /// <summary>
    /// Adds a new <paramref name="car"/> entry.
    /// </summary>
    /// <param name="car">The <see cref="CarDao"/> to add.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task AddAsync(CarDao car);

    /// <summary>
    /// Deletes the car with the provided <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The id to delete by.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Gets the <see cref="CarDao"/> by the <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The id to search by.</param>
    /// <returns></returns>
    Task<CarDao> GetAsync(Guid id);

    /// <summary>
    /// Gets all <see cref="CarDao"/> entries registered by the user with
    /// the provided <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">The id of the used to search by.</param>
    /// <param name="page">The page to display, starting from 1.</param>
    /// <param name="count">The number of items per page.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="CarDao"/>.</returns>
    Task<IEnumerable<CarDao>> GetAllByUserAsync(Guid userId, int page = 1, int count = 10);

    /// <summary>
    /// Updates the provided <see cref="CarDao"/> entry.
    /// </summary>
    /// <param name="car">The car with the new data.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task UpdateAsync(CarDao car);
}
