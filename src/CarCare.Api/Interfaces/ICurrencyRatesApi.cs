using CarCare.Api.Models.Dao;

namespace CarCare.Api.Interfaces;

/// <summary>
/// Manages exchange rates between currencies.
/// </summary>
public interface ICurrencyRatesApi
{
    /// <summary>
    /// Gets the rate between the <paramref name="fromCurrency"/> and <paramref name="toCurrency"/>
    /// for the current date.
    /// </summary>
    /// <param name="fromCurrency">The source currency.</param>
    /// <param name="toCurrency">The target currency.</param>
    /// <returns>A new <see cref="CurrencyRateApiModel"/> object.</returns>
    Task<CurrencyRateApiModel> GetAsync(string fromCurrency, string toCurrency);

    /// <summary>
    /// Gets the rate between the <paramref name="fromCurrency"/> and <paramref name="toCurrency"/>
    /// for the provided <paramref name="date"/>.
    /// </summary>
    /// <param name="fromCurrency">The source currency.</param>
    /// <param name="toCurrency">The target currency.</param>
    /// <param name="date">The <see cref="DateOnly"/> to fetch rates for.</param>
    /// <returns>A new <see cref="CurrencyRateApiModel"/> object.</returns>
    Task<CurrencyRateApiModel> GetAsync(string fromCurrency, string toCurrency, DateOnly date);
}
