using CarCare.Processing.Contexts;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarCare.Processing.Extensions;

/// <summary>
/// Provides extensions to the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Registers all Processing level services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    public static void AddProcessingServices(this IServiceCollection services)
    {
        services.AddSingleton<IDatabaseManagementService, DatabaseManagementService>();
        services.AddScoped<ISplashScreenContext, SplashScreenContext>();
    }
}
