using CarCare.Api.Interfaces;
using CarCare.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarCare.Api.Extensions;

/// <summary>
/// Provides extensions to the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Registers all API level services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    public static void AddApiServices(this IServiceCollection services)
    {
        services.AddHttpClient<RatesDbCurrencyRatesApi>(
            c => c.BaseAddress = new("https://free.ratesdb.com/v1"));
        services.AddScoped<ICurrencyRatesApi, RatesDbCurrencyRatesApi>();
    }
}
