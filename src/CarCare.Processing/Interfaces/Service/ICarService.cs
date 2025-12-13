using CarCare.Processing.Models.Core;

namespace CarCare.Processing.Interfaces.Service;

internal interface ICarService
{
    Task AddAsync(Car car);

    Task DeleteAsync(Guid id);

    Task<Car> GetAsync(Guid id);

    Task<IEnumerable<Car>> GetAllByUserIdAsync(Guid userId, int page = 1, int count = 10);

    Task UpdateAsync(Car car);
}
