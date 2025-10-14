using CarCare.Processing.Interfaces.Service;
using Microsoft.Extensions.DependencyInjection;

namespace CarCare.UserInterface.Extensions;

/// <summary>
/// Provides extension to <see cref="IServiceProvider"/>.
/// </summary>
public static class IServiceProviderExtensions
{
    /// <summary>
    /// Initialises the application state.
    /// </summary>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> of the app.</param>
    public static void InitialiseApplication(this IServiceProvider serviceProvider)
    {
        // set the default theme
        IThemeService themeService = serviceProvider.GetRequiredService<IThemeService>();
        themeService.SetDefaultTheme();
    }
}
