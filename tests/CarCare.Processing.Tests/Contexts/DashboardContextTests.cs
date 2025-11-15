using CarCare.Processing.Contexts;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace CarCare.Processing.Tests.Contexts;

public class DashboardContextTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationService _navigationService;
    private readonly ISettingsContext _settingsContext;
    private readonly DashboardContext _sut;

    public DashboardContextTests()
    {
        _serviceProvider = Substitute.For<IServiceProvider>();
        _navigationService = Substitute.For<INavigationService>();
        _settingsContext = Substitute.For<ISettingsContext>();
        _sut = new(_serviceProvider, _navigationService);
    }

    [Fact]
    public void ShouldNavigateToSettings()
    {
        // Arrange
        _navigationService.NavigateTo<ISettingsContext>();
        _serviceProvider.GetService(typeof(ISettingsContext)).Returns(_settingsContext);

        // Act
        _sut.OpenAppSettingsCommand.Execute(null);

        // Assert
        _navigationService.Received().NavigateTo<ISettingsContext>();
        _serviceProvider.Received().GetService(typeof(ISettingsContext));
        _settingsContext.Received().Tab = Enums.SettingsTab.ApplicationSettings;
    }
}
