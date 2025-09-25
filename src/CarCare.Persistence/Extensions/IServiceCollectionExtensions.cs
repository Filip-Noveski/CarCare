using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Services;
using CarCare.Persistence.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace CarCare.Persistence.Extensions;

/// <summary>
/// Provides extensions to the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Registers all Persistence level services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    public static void AddPersistenceServices(this IServiceCollection services)
    {
        services.AddSingleton<DBContext>();
        services.AddTransient<IMigrationsHistoryRepository, MigrationsHistoryRepository>();
    }
}
