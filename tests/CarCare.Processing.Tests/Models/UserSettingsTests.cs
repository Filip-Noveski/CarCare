using CarCare.Persistence.Models;
using CarCare.Processing.Models.Core;
using CarCare.Processing.Models.Dto;
using FluentAssertions;

namespace CarCare.Processing.Tests.Models;

public class UserSettingsTests
{
    [Fact]
    public void ShouldCreateDefaultSettings()
    {
        // Arrange
        Guid id = Guid.NewGuid();

        // Act
        UserSettings settings = UserSettings.CreateDefault(id);

        // Assert
        settings.UserId.Should().Be(id);
    }

    [Fact]
    public void ShouldMapToDto()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        UserSettings settings = new(id);

        // Act
        UserSettingsDto dto = settings.ToDto();

        // Assert
        dto.UserId.Should().Be(id);
    }

    [Fact]
    public void ShouldMapToDao()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        UserSettings settings = new(id);

        // Act
        UserSettingsDao dao = settings.ToDao();

        // Assert
        dao.UserId.Should().Be(id);
    }
}
