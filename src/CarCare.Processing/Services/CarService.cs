using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using CarCare.Processing.Enums;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models.Core;

namespace CarCare.Processing.Services;

internal class CarService : ICarService
{
    private readonly ICarRepository _carRepository;
    private readonly ICarLifecycleRepository _carLifecycleRepository;
    private readonly IUserSession _session;
    private readonly IBitmapCreatorService _bitmapService;

    public CarService(
        ICarRepository carRepository,
        IUserSession session,
        IBitmapCreatorService bitmapService,
        ICarLifecycleRepository carLifecycleRepository)
    {
        _carRepository = carRepository;
        _session = session;
        _bitmapService = bitmapService;
        _carLifecycleRepository = carLifecycleRepository;
    }

    public async Task AddAsync(Car car)
    {
        if (!_session.IsAuthenticated(car.UserId))
        {
            return;
        }

        await _carRepository.AddAsync(car.ToDao(_bitmapService));
        await _carLifecycleRepository.AddAsync(car.LifecycleToDao());
    }

    public async Task DeleteAsync(Guid id)
    {
        CarDao old = await _carRepository.GetAsync(id);
        if (!_session.IsAuthenticated(old.UserId))
        {
            return;
        }

        await _carLifecycleRepository.DeleteAsync(id);
        await _carRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Car>> GetAllByUserIdAsync(Guid userId, int page = 1, int count = 10)
    {
        IEnumerable<CarDao> result = await _carRepository.GetAllByUserAsync(userId, page, count);
        IEnumerable<Task<Car>> tasks = result.Select(async x =>
        {
            CarLifecycleDao lifecycleDao = await _carLifecycleRepository.GetAsync(x.Id);
            CarLifecycle lifecycle = new(
                lifecycleDao.PurchaseDate,
                lifecycleDao.PurchasePrice,
                Enum.Parse<Currency>(lifecycleDao.PurchaseCurrency, ignoreCase: true),
                lifecycleDao.SellDate,
                lifecycleDao.SellPrice,
                lifecycleDao.SellCurrency is null 
                    ? null 
                    : Enum.Parse<Currency>(lifecycleDao.SellCurrency, ignoreCase: true));

            return new Car(
                x.Id,
                x.UserId,
                x.Manufacturer,
                x.Model,
                x.Specification,
                x.ModelYear,
                x.Image is null ? null : _bitmapService.ConvertToBitmap(x.Image),
                lifecycle);
        });

        return await Task.WhenAll(tasks);
    }

    public async Task<Car> GetAsync(Guid id)
    {
        CarDao dao = await _carRepository.GetAsync(id);
        CarLifecycleDao lifecycleDao = await _carLifecycleRepository.GetAsync(id);
        CarLifecycle lifecycle = new(
            lifecycleDao.PurchaseDate,
            lifecycleDao.PurchasePrice,
            Enum.Parse<Currency>(lifecycleDao.PurchaseCurrency, ignoreCase: true),
            lifecycleDao.SellDate,
            lifecycleDao.SellPrice,
            lifecycleDao.SellCurrency is null
                ? null
                : Enum.Parse<Currency>(lifecycleDao.SellCurrency, ignoreCase: true));

        return new(
            dao.Id,
            dao.UserId,
            dao.Manufacturer,
            dao.Model,
            dao.Specification,
            dao.ModelYear,
            dao.Image is null ? null : _bitmapService.ConvertToBitmap(dao.Image),
            lifecycle);
    }

    public async Task UpdateAsync(Car car)
    {
        if (!_session.IsAuthenticated(car.UserId))
        {
            return;
        }

        await _carRepository.UpdateAsync(car.ToDao(_bitmapService));
        await _carLifecycleRepository.UpdateAsync(car.LifecycleToDao());
    }
}
