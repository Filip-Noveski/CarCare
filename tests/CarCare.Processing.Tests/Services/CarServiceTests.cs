using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
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
    private readonly IUserSession _session;
    private readonly IBitmapCreatorService _bitmapService;
    private readonly CarService _sut;

    public CarServiceTests()
    {
        _carRepository = Substitute.For<ICarRepository>();
        _session = Substitute.For<IUserSession>();
        _bitmapService = Substitute.For<IBitmapCreatorService>();
        _sut = new(_carRepository, _session, _bitmapService);
    }

    [Fact]
    public async Task ShouldAddNewCar()
    {
        // Arrange
        Car car = new(Guid.NewGuid(), Guid.NewGuid(), "Ford", "Fiesta", "ST", 2023);
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
    }

    [Fact]
    public async Task ShouldNotAddCarIfUserNotAuthenticated()
    {
        // Arrange
        Car car = new(Guid.NewGuid(), Guid.NewGuid(), "Ford", "Fiesta", "ST", 2023);
        _session.IsAuthenticated(car.UserId).Returns(false);

        // Act
        await _sut.AddAsync(car);

        // Assert
        _session.Received().IsAuthenticated(car.UserId);
        await _carRepository.DidNotReceive().AddAsync(Arg.Any<CarDao>());
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

        // Act
        IEnumerable<Car> result = await _sut.GetAllByUserIdAsync(userId);

        // Assert
        await _carRepository.Received().GetAllByUserAsync(userId);
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
        }
    }

    [Fact]
    public async Task ShouldGetSingleCar()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        CarDao car = new(id, Guid.NewGuid(), "Ford", "Fiesta", "ST", 2023);
        _carRepository.GetAsync(id).Returns(car);

        // Act
        Car result = await _sut.GetAsync(id);

        // Assert
        await _carRepository.Received().GetAsync(id);
        result.Should().Match<Car>(
            x => x.Id == car.Id
                && x.UserId == car.UserId
                && x.Manufacturer == car.Manufacturer
                && x.Model == car.Model
                && x.Specification == car.Specification
                && x.ModelYear == car.ModelYear
                && x.Image == null);
    }

    [Fact]
    public async Task ShouldUpdateCar()
    {
        // Arrange
        Car car = new(Guid.NewGuid(), Guid.NewGuid(), "Ford", "Fiesta", "ST", 2023);
        _session.IsAuthenticated(car.UserId).Returns(true);
        _carRepository.UpdateAsync(Arg.Any<CarDao>()).Returns(Task.CompletedTask);

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
    }

    [Fact]
    public async Task ShouldNotUpdateCarIfUserNotAuthenticated()
    {
        // Arrange
        Car car = new(Guid.NewGuid(), Guid.NewGuid(), "Ford", "Fiesta", "ST", 2023);
        _session.IsAuthenticated(car.UserId).Returns(false);

        // Act
        await _sut.UpdateAsync(car);

        // Assert
        _session.Received().IsAuthenticated(car.UserId);
        await _carRepository.DidNotReceive().UpdateAsync(Arg.Any<CarDao>());
    }
}
