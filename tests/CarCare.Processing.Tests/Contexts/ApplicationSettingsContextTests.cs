using CarCare.Processing.Contexts;
using CarCare.Processing.Enums;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models.Core;
using CarCare.Processing.Models.Dto;
using NSubstitute;

namespace CarCare.Processing.Tests.Contexts;

public class ApplicationSettingsContextTests
{
    private readonly IThemeService _themeService;
    private readonly IUserSettingsService _userSettingsService;
    private readonly IUserSession _session;
    private readonly ApplicationSettingsContext _sut;

    public ApplicationSettingsContextTests()
    {
        _themeService = Substitute.For<IThemeService>();
        _userSettingsService = Substitute.For<IUserSettingsService>();
        _session = Substitute.For<IUserSession>();
        _sut = new(_themeService, _userSettingsService, _session);
    }

    [Theory]
    [InlineData(ApplicationTheme.Dark, ApplicationTheme.Light)]
    [InlineData(ApplicationTheme.Dark, ApplicationTheme.Dark)]
    [InlineData(ApplicationTheme.Light, ApplicationTheme.Light)]
    [InlineData(ApplicationTheme.Light, ApplicationTheme.Dark)]
    public async Task ShouldUpdateThemeToLight(ApplicationTheme oldTheme, ApplicationTheme newTheme)
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        _sut.Theme = newTheme;
        _themeService.ChangeTheme(newTheme);
        _session.User.Returns(new UserDto(userId, "Irrelevant", new()));
        _userSettingsService.GetAsync(userId).Returns(new UserSettings(userId, oldTheme));
        _userSettingsService.UpdateAsync(Arg.Any<UserSettings>()).Returns(Task.CompletedTask);

        // Act
        _sut.UpdateThemeCommand.Execute(null);

        // Assert
        _themeService.Received().ChangeTheme(newTheme);
        await _userSettingsService.Received().GetAsync(userId);
        await _userSettingsService.Received().UpdateAsync(Arg.Is<UserSettings>(
            s => s.UserId == userId && s.Theme == newTheme));
    }
}
