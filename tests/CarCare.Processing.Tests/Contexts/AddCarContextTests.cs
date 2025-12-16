using CarCare.Processing.Contexts;
using CarCare.Processing.Enums;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models.Core;
using CarCare.Processing.Models.Dto;
using FluentAssertions;
using NSubstitute;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CarCare.Processing.Tests.Contexts;

public class AddCarContextTests
{
    private readonly ICarService _carService;
    private readonly IUserSession _session;
    private readonly IEventManagerService _eventManager;
    private readonly IFileDialogueService _fileService;
    private readonly AddCarContext _sut;

    public AddCarContextTests()
    {
        _carService = Substitute.For<ICarService>();
        _session = Substitute.For<IUserSession>();
        _eventManager = Substitute.For<IEventManagerService>();
        _fileService = Substitute.For<IFileDialogueService>();
        _sut = new(_carService, _session, _eventManager, _fileService);
    }

    [Fact]
    public async Task ShouldAddNewCar()
    {
        // Arrange
        _sut.Manufacturer = "Renault";
        _sut.Model = "Clio";
        _sut.Specification = "RS 197";
        _sut.ModelYear = "2008";
        _sut.PurchaseDate = new(2023, 1, 8);
        _sut.PurchasePrice = "15_000";
        _sut.PurchaseCurrency = Currency.Eur;
        UserDto user = new(Guid.NewGuid(), string.Empty, null!);
        _session.User.Returns(user);
        _carService.AddAsync(Arg.Any<Car>()).Returns(Task.CompletedTask);
        _eventManager.OnMyCarsChanged();

        // Act
        Thread thread = new(() => _sut.AddCarCommand.Execute(new Window()));
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        // Assert
        await _carService.Received().AddAsync(Arg.Is<Car>(
            x => x.Id != Guid.Empty
            && x.UserId == user.Id
            && x.Manufacturer == "Renault"
            && x.Model == "Clio"
            && x.Specification == "RS 197"
            && x.ModelYear == 2008
            && x.Image == null
            && x.Lifecycle.PurchaseDate == new DateOnly(2023, 1, 8)
            && x.Lifecycle.PurchasePrice == 15_000
            && x.Lifecycle.PurchaseCurrency == Currency.Eur
            && x.Lifecycle.SaleDate == null
            && x.Lifecycle.SalePrice == null
            && x.Lifecycle.SaleCurrency == null));
        _eventManager.Received().OnMyCarsChanged();
    }

    [Fact]
    public async Task ShouldFailOnManufacturer()
    {
        // Arrange
        _sut.Manufacturer = "";
        _sut.Model = "Clio";
        _sut.Specification = "RS 197";
        _sut.ModelYear = "2008";
        _sut.PurchaseDate = new(2023, 1, 8);
        _sut.PurchasePrice = "15_000";
        _sut.PurchaseCurrency = Currency.Eur;
        UserDto user = new(Guid.NewGuid(), string.Empty, null!);
        _session.User.Returns(user);

        // Act
        Thread thread = new(() => _sut.AddCarCommand.Execute(new Window()));
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        // Assert
        await _carService.DidNotReceive().AddAsync(Arg.Any<Car>());
        _eventManager.DidNotReceive().OnMyCarsChanged();
        _sut.ManufacturerError.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task ShouldFailOnModel()
    {
        // Arrange
        _sut.Manufacturer = "Renault";
        _sut.Model = "";
        _sut.Specification = "RS 197";
        _sut.ModelYear = "2008";
        _sut.PurchaseDate = new(2023, 1, 8);
        _sut.PurchasePrice = "15_000";
        _sut.PurchaseCurrency = Currency.Eur;
        UserDto user = new(Guid.NewGuid(), string.Empty, null!);
        _session.User.Returns(user);

        // Act
        Thread thread = new(() => _sut.AddCarCommand.Execute(new Window()));
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        // Assert
        await _carService.DidNotReceive().AddAsync(Arg.Any<Car>());
        _eventManager.DidNotReceive().OnMyCarsChanged();
        _sut.ModelError.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void ShouldOpenFileDialogue()
    {
        // Arrange
        BitmapImage image = new();
        _fileService.GetImageFile().Returns(image);

        // Act
        _sut.ChooseMainImageCommand.Execute(null);

        // Assert
        _fileService.Received().GetImageFile();
        _sut.MainImage.Should().Be(image);
    }

    [Fact]
    public void ShouldDeleteMainImage()
    {
        // Arrange
        _sut.MainImage = new();

        // Act
        _sut.DeleteMainImageCommand.Execute(null);

        // Assert
        _sut.MainImage.Should().BeNull();
    }
}
