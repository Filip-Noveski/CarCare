using CarCare.Processing.Contexts;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using NSubstitute;

namespace CarCare.Processing.Tests.Contexts;

public class MyCarsContextTests
{
    private readonly ICarService _carService;
    private readonly IUserSession _session;
    private readonly IBitmapCreatorService _bitmapService;
    private readonly INavigationService _navigationService;
    private readonly IEventManagerService _eventManager;
    private readonly MyCarsContext _sut;

    public MyCarsContextTests()
    {
        _carService = Substitute.For<ICarService>();
        _session = Substitute.For<IUserSession>();
        _bitmapService = Substitute.For<IBitmapCreatorService>();
        _navigationService = Substitute.For<INavigationService>();
        _eventManager = Substitute.For<IEventManagerService>();
        _sut = new(_carService, _session, _bitmapService, _navigationService, _eventManager);
    }

    [Fact]
    public void ShouldNavigateToAddNewCarView()
    {
        // Arrange
        _navigationService.NavigateTo<IAddCarContext>();

        // Act
        _sut.AddNewCarCommand.Execute(null);

        // Assert
        _navigationService.Received().NavigateTo<IAddCarContext>();
    }
}
