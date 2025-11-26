using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models.Core;

namespace CarCare.Processing.Services;

internal class CarService : ICarService
{
    private readonly ICarRepository _carRepository;
    private readonly IUserSession _session;
    private readonly IBitmapCreatorService _bitmapService;

    public CarService(
        ICarRepository carRepository,
        IUserSession session,
        IBitmapCreatorService bitmapService)
    {
        _carRepository = carRepository;
        _session = session;
        _bitmapService = bitmapService;
    }

    public async Task AddAsync(Car car)
    {
        if (!_session.IsAuthenticated(car.UserId))
        {
            return;
        }

        await _carRepository.AddAsync(car.ToDao(_bitmapService));
    }

    public async Task DeleteAsync(Guid id)
    {
        CarDao old = await _carRepository.GetAsync(id);
        if (!_session.IsAuthenticated(old.UserId))
        {
            return;
        }

        await _carRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Car>> GetAllByUserIdAsync(Guid userId)
    {
        IEnumerable<CarDao> result = await _carRepository.GetAllByUserAsync(userId);
        return result.Select(
            x => new Car(
                x.Id,
                x.UserId,
                x.Manufacturer,
                x.Model,
                x.Specification,
                x.ModelYear,
                x.Image is null ? null : _bitmapService.ConvertToBitmap(x.Image)));
    }

    public async Task<Car> GetAsync(Guid id)
    {
        CarDao dao = await _carRepository.GetAsync(id);
        return new(
            dao.Id,
            dao.UserId,
            dao.Manufacturer,
            dao.Model,
            dao.Specification,
            dao.ModelYear,
            dao.Image is null ? null : _bitmapService.ConvertToBitmap(dao.Image));
    }

    public async Task UpdateAsync(Car car)
    {
        if (!_session.IsAuthenticated(car.UserId))
        {
            return;
        }

        await _carRepository.UpdateAsync(car.ToDao(_bitmapService));
    }
}
