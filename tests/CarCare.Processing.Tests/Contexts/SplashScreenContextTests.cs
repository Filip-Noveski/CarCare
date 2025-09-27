using CarCare.Processing.Contexts;
using CarCare.Processing.Interfaces.Service;
using NSubstitute;

namespace CarCare.Processing.Tests.Contexts;

public class SplashScreenContextTests
{
    private readonly IDatabaseManagementService _dbManagementService;
    private readonly SplashScreenContext _sut;

    public SplashScreenContextTests()
    {
        _dbManagementService = Substitute.For<IDatabaseManagementService>();
        _sut = new(_dbManagementService);
    }

    [Fact]
    public async Task ShouldInitialiseDatabase()
    {
        // Arrange
        _dbManagementService.VerifyDatabaseStateAsync().Returns(Task.CompletedTask);

        // Act
        await _sut.InitialiseDatabaseAsync();

        // Assert
        await _dbManagementService.Received(1).VerifyDatabaseStateAsync();
    }
}
