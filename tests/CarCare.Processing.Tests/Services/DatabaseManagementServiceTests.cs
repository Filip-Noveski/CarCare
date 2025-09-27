using CarCare.Persistence.Interfaces;
using CarCare.Processing.Services;
using NSubstitute;

namespace CarCare.Processing.Tests.Services;

public class DatabaseManagementServiceTests
{
    private readonly IDatabaseManager _manager;
    private readonly DatabaseManagementService _sut;

    public DatabaseManagementServiceTests()
    {
        _manager = Substitute.For<IDatabaseManager>();
        _sut = new(_manager);
    }

    [Fact]
    public async Task ShouldForwardVerificationCallToManager()
    {
        // Arrage
        _manager.VerifyDatabaseStateAsync().Returns(Task.CompletedTask);

        // Act
        await _sut.VerifyDatabaseStateAsync();

        // Assert
        await _manager.Received(1).VerifyDatabaseStateAsync();
    }
}
