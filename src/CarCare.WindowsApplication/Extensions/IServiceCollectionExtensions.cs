using Microsoft.Extensions.DependencyInjection;

namespace CarCare.WindowsApplication.Extensions;

/// <summary>
/// Provides extensions to the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Registers all WindowsApplication level services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    public static void AddWindowsApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
    }
}
