using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using CarCare.Processing.Enums;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models.Core;
using CarCare.Processing.Services;
using FluentAssertions;
using NSubstitute;

namespace CarCare.Processing.Tests.Services;

public class CarServiceTests
{
    private readonly ICarRepository _carRepository;
    private readonly ICarLifecycleRepository _carLifecycleRepository;
    private readonly IUserSession _session;
    private readonly IBitmapCreatorService _bitmapService;
    private readonly CarService _sut;

    public CarServiceTests()
    {
        _carRepository = Substitute.For<ICarRepository>();
        _session = Substitute.For<IUserSession>();
        _bitmapService = Substitute.For<IBitmapCreatorService>();
        _carLifecycleRepository = Substitute.For<ICarLifecycleRepository>();
        _sut = new(_carRepository, _session, _bitmapService, _carLifecycleRepository);
    }

    [Fact]
    public async Task ShouldAddNewCar()
    {
        // Arrange
        CarLifecycle lifecycle = new(new(2022, 1, 15), 20000, Currency.Usd);
        Car car = new(Guid.NewGuid(), Guid.NewGuid(), "Ford", "Fiesta", "ST", 2023, lifecycle);
        _session.IsAuthenticated(car.UserId).Returns(true);
        _carRepository.AddAsync(Arg.Any<CarDao>()).Returns(Task.CompletedTask);

        // Act
        await _sut.AddAsync(car);

        // Assert
        _session.Received().IsAuthenticated(car.UserId);
        await _carRepository.Received().AddAsync(Arg.Is<CarDao>(
            x => x.Id == car.Id
            && x.UserId == car.UserId
            && x.Manufacturer == car.Manufacturer
            && x.Model == car.Model
            && x.Specification == car.Specification
            && x.ModelYear == car.ModelYear));
        await _carLifecycleRepository.Received().AddAsync(Arg.Is<CarLifecycleDao>(
            x => x.CarId == car.Id
            && x.PurchaseDate == lifecycle.PurchaseDate
            && x.PurchasePrice == lifecycle.PurchasePrice
            && x.PurchaseCurrency == "USD"
            && x.SellDate == null
            && x.SellPrice == null
            && x.SellCurrency == null));
    }

    [Fact]
    public async Task ShouldNotAddCarIfUserNotAuthenticated()
    {
        // Arrange
        CarLifecycle lifecycle = new(new(2022, 1, 15), 20000, Currency.Usd);
        Car car = new(Guid.NewGuid(), Guid.NewGuid(), "Ford", "Fiesta", "ST", 2023, lifecycle);
        _session.IsAuthenticated(car.UserId).Returns(false);

        // Act
        await _sut.AddAsync(car);

        // Assert
        _session.Received().IsAuthenticated(car.UserId);
        await _carRepository.DidNotReceive().AddAsync(Arg.Any<CarDao>());
        await _carLifecycleRepository.DidNotReceive().AddAsync(Arg.Any<CarLifecycleDao>());
    }

    [Fact]
    public async Task ShouldDeleteCar()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        CarDao car = new(id, Guid.NewGuid(), "Ford", "Fiesta", "ST", 2023);
        _session.IsAuthenticated(car.UserId).Returns(true);
        _carRepository.DeleteAsync(id).Returns(Task.CompletedTask);
        _carRepository.GetAsync(id).Returns(car);

        // Act
        await _sut.DeleteAsync(id);

        // Assert
        _session.Received().IsAuthenticated(car.UserId);
        await _carRepository.Received().GetAsync(id);
        await _carRepository.Received().DeleteAsync(id);
        await _carLifecycleRepository.Received().DeleteAsync(id);
    }

    [Fact]
    public async Task ShouldNotDeleteCarIfUserNotAuthenticated()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        CarDao car = new(id, Guid.NewGuid(), "Ford", "Fiesta", "ST", 2023);
        _session.IsAuthenticated(car.UserId).Returns(false);
        _carRepository.GetAsync(id).Returns(car);

        // Act
        await _sut.DeleteAsync(id);

        // Assert
        _session.Received().IsAuthenticated(car.UserId);
        await _carRepository.Received().GetAsync(id);
        await _carRepository.DidNotReceive().DeleteAsync(id);
        await _carLifecycleRepository.DidNotReceive().DeleteAsync(id);
    }

    [Fact]
    public async Task ShouldGetCars()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        CarDao[] cars = new[]
        {
            new CarDao(Guid.NewGuid(), userId, "Ford", "Fiesta", "ST", 2023),
            new CarDao(Guid.NewGuid(), userId, "Renault", "Clio", "RS 197", 2008)
        };
        _carRepository.GetAllByUserAsync(userId).Returns(cars);
        CarLifecycleDao lifecycle0 = new(cars[0].Id, new(2022, 1, 15), 20000, "USD", null, null, null);
        CarLifecycleDao lifecycle1 = new(cars[1].Id, new(2010, 5, 20), 15000, "EUR",
            new(2021, 3, 8), 17500, "EUR");
        _carLifecycleRepository.GetAsync(cars[0].Id).Returns(lifecycle0);
        _carLifecycleRepository.GetAsync(cars[1].Id).Returns(lifecycle1);

        // Act
        IEnumerable<Car> result = await _sut.GetAllByUserIdAsync(userId);

        // Assert
        await _carRepository.Received().GetAllByUserAsync(userId);
        await _carLifecycleRepository.Received().GetAsync(cars[0].Id);
        await _carLifecycleRepository.Received().GetAsync(cars[1].Id);
        result.Should().HaveCount(2);
        for (int i = 0; i < cars.Length; i++)
        {
            result.ElementAt(i).Should().Match<Car>(
                x => x.Id == cars[i].Id
                    && x.UserId == cars[i].UserId
                    && x.Manufacturer == cars[i].Manufacturer
                    && x.Model == cars[i].Model
                    && x.Specification == cars[i].Specification
                    && x.ModelYear == cars[i].ModelYear
                    && x.Image == null);
            result.ElementAt(i).Lifecycle.Should().Match<CarLifecycle>(
                x => x.PurchaseDate == (i == 0 ? lifecycle0.PurchaseDate : lifecycle1.PurchaseDate)
                    && x.PurchasePrice == (i == 0 ? lifecycle0.PurchasePrice : lifecycle1.PurchasePrice)
                    && x.PurchaseCurrency == (i == 0 ? Currency.Usd : Currency.Eur)
                    && x.SaleDate == (i == 0 ? lifecycle0.SellDate : lifecycle1.SellDate)
                    && x.SalePrice == (i == 0 ? lifecycle0.SellPrice : lifecycle1.SellPrice)
                    && x.SaleCurrency == (i == 0 ? null : Currency.Eur));
        }
    }

    [Fact]
    public async Task ShouldGetSingleCar()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        CarDao car = new(id, Guid.NewGuid(), "Ford", "Fiesta", "ST", 2023);
        _carRepository.GetAsync(id).Returns(car);
        CarLifecycleDao lifecycle = new(car.Id, new(2022, 1, 15), 20000, "USD", null, null, null);
        _carLifecycleRepository.GetAsync(car.Id).Returns(lifecycle);

        // Act
        Car result = await _sut.GetAsync(id);

        // Assert
        await _carRepository.Received().GetAsync(id);
        await _carLifecycleRepository.Received().GetAsync(car.Id);
        result.Should().Match<Car>(
            x => x.Id == car.Id
                && x.UserId == car.UserId
                && x.Manufacturer == car.Manufacturer
                && x.Model == car.Model
                && x.Specification == car.Specification
                && x.ModelYear == car.ModelYear
                && x.Image == null);
        result.Lifecycle.Should().Match<CarLifecycle>(
            x => x.PurchaseDate == lifecycle.PurchaseDate
                && x.PurchasePrice == lifecycle.PurchasePrice
                && x.PurchaseCurrency == Currency.Usd
                && x.SaleDate == lifecycle.SellDate
                && x.SalePrice == lifecycle.SellPrice
                && x.SaleCurrency == null);
    }

    [Fact]
    public async Task ShouldUpdateCar()
    {
        // Arrange
        CarLifecycle lifecycle = new(new(2024, 1, 15), 20000, Currency.Gbp, null, null, null);
        Car car = new(Guid.NewGuid(), Guid.NewGuid(), "Ford", "Fiesta", "ST", 2023, lifecycle);
        _session.IsAuthenticated(car.UserId).Returns(true);
        _carRepository.UpdateAsync(Arg.Any<CarDao>()).Returns(Task.CompletedTask);
        _carLifecycleRepository.UpdateAsync(Arg.Any<CarLifecycleDao>()).Returns(Task.CompletedTask);

        // Act
        await _sut.UpdateAsync(car);

        // Assert
        _session.Received().IsAuthenticated(car.UserId);
        await _carRepository.Received().UpdateAsync(Arg.Is<CarDao>(
            x => x.Id == car.Id
            && x.UserId == car.UserId
            && x.Manufacturer == car.Manufacturer
            && x.Model == car.Model
            && x.Specification == car.Specification
            && x.ModelYear == car.ModelYear));
        await _carLifecycleRepository.Received().UpdateAsync(Arg.Is<CarLifecycleDao>(
            x => x.CarId == car.Id
            && x.PurchaseDate == lifecycle.PurchaseDate
            && x.PurchasePrice == lifecycle.PurchasePrice
            && x.PurchaseCurrency == "GBP"
            && x.SellDate == null
            && x.SellPrice == null
            && x.SellCurrency == null));
    }

    [Fact]
    public async Task ShouldNotUpdateCarIfUserNotAuthenticated()
    {
        // Arrange
        CarLifecycle lifecycle = new(new(2024, 1, 15), 20000, Currency.Gbp, null, null, null);
        Car car = new(Guid.NewGuid(), Guid.NewGuid(), "Ford", "Fiesta", "ST", 2023, lifecycle);
        _session.IsAuthenticated(car.UserId).Returns(false);

        // Act
        await _sut.UpdateAsync(car);

        // Assert
        _session.Received().IsAuthenticated(car.UserId);
        await _carRepository.DidNotReceive().UpdateAsync(Arg.Any<CarDao>());
        await _carLifecycleRepository.DidNotReceive().UpdateAsync(Arg.Any<CarLifecycleDao>());
    }
}
