using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using CarCare.Processing.Enums;
using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models.Core;
using CarCare.Processing.Services;
using FluentAssertions;
using NSubstitute;

namespace CarCare.Processing.Tests.Services;

public class UserSettingsServiceTests
{
    private readonly IUserSettingsRepository _settingsRepository;
    private readonly IUserSession _session;
    private readonly UserSettingsService _sut;

    public UserSettingsServiceTests()
    {
        _settingsRepository = Substitute.For<IUserSettingsRepository>();
        _session = Substitute.For<IUserSession>();
        _sut = new(_settingsRepository, _session);
    }

    [Fact]
    public async Task ShouldRequestSettingsCreation()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        UserSettings settings = new(id, ApplicationTheme.Light, Currency.Eur);
        _settingsRepository.AddAsync(Arg.Any<UserSettingsDao>()).Returns(Task.CompletedTask);

        // Act
        await _sut.AddAsync(settings);

        // Assert
        await _settingsRepository.Received().AddAsync(Arg.Is<UserSettingsDao>(
                x => x.UserId == id && x.Theme == "Light" && x.PreferredCurrency == "EUR"));
    }

    [Fact]
    public async Task ShouldRequestSettingsDeletionOnAuth()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        _session.IsAuthenticated(id).Returns(true);
        _settingsRepository.DeleteAsync(id).Returns(Task.CompletedTask);

        // Act
        await _sut.DeleteAsync(id);

        // Assert
        await _settingsRepository.Received().DeleteAsync(id);
        _session.Received().IsAuthenticated(id);
    }

    [Fact]
    public async Task ShouldNotRequestSettingsDeletionWithoutAuth()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        _session.IsAuthenticated(id).Returns(false);
        _settingsRepository.DeleteAsync(id).Returns(Task.CompletedTask);

        // Act
        await _sut.DeleteAsync(id);

        // Assert
        await _settingsRepository.DidNotReceive().DeleteAsync(Arg.Any<Guid>());
        _session.Received().IsAuthenticated(id);
    }

    [Fact]
    public async Task ShouldGetSettings()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        UserSettingsDao dao = new(id, "Light", "GBP");
        _settingsRepository.GetAsync(id).Returns(dao);

        // Act
        UserSettings result = await _sut.GetAsync(id);

        // Assert
        await _settingsRepository.Received().GetAsync(id);
        result.Should().BeEquivalentTo(new UserSettings(id, ApplicationTheme.Light, Currency.Gbp));
    }

    [Fact]
    public async Task ShouldRequestUpdateOnAuth()
    {
        // Arrange
        UserSettings settings = new(Guid.NewGuid(), ApplicationTheme.Light, Currency.Gbp);
        _settingsRepository.UpdateAsync(Arg.Any<UserSettingsDao>()).Returns(Task.CompletedTask);
        _session.IsAuthenticated(settings.UserId).Returns(true);

        // Act
        await _sut.UpdateAsync(settings);

        // Assert
        await _settingsRepository.Received().UpdateAsync(Arg.Is<UserSettingsDao>(
            x => x.UserId == settings.UserId && x.Theme == "Light" && x.PreferredCurrency == "GBP"));
        _session.Received().IsAuthenticated(settings.UserId);
    }

    [Fact]
    public async Task ShouldNotRequestUpdateWithoutAuth()
    {
        // Arrange
        UserSettings settings = new(Guid.NewGuid(), ApplicationTheme.Light, Currency.Gbp);
        _settingsRepository.UpdateAsync(Arg.Any<UserSettingsDao>()).Returns(Task.CompletedTask);
        _session.IsAuthenticated(settings.UserId).Returns(false);

        // Act
        await _sut.UpdateAsync(settings);

        // Assert
        await _settingsRepository.DidNotReceive().UpdateAsync(Arg.Any<UserSettingsDao>());
        _session.Received().IsAuthenticated(settings.UserId);
    }
}
