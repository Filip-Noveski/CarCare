using CarCare.UserInterface.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace CarCare.UserInterface.Extensions;

/// <summary>
/// Provides extensions to the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Registers all UserInterface level services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    public static void AddUserInterfaceServices(this IServiceCollection services)
    {
        services.AddSingleton<Splash>();
        services.AddSingleton<Dashboard>();
    }
}
