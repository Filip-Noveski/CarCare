using CarCare.Persistence.Models;

namespace CarCare.Persistence.Interfaces;

/// <summary>
/// A repository for performing CRUD operations with the
/// CurrencyRatesCache table.
/// </summary>
public interface ICurrencyRatesCacheRepository
{
    /// <summary>
    /// Returns the <see cref="CurrencyRateDao"/> for the provided <paramref name="date"/>
    /// if it is cached. Otherwise, null is returned.
    /// </summary>
    /// <param name="fromCurrency">The source currency for conversion.</param>
    /// <param name="toCurrency">The target currency for conversion.</param>
    /// <param name="date">The date for the rate.</param>
    /// <returns>A <see cref="CurrencyRateDao"/> or null.</returns>
    Task<CurrencyRateDao?> GetIfExistsAsync(string fromCurrency, string toCurrency, DateOnly date);

    /// <summary>
    /// Caches the provided <paramref name="currencyRateDao"/> and evicts and older entry if required.
    /// </summary>
    /// <param name="currencyRateDao">The <see cref="CurrencyRateDao"/> to cache.</param>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task AddAsync(CurrencyRateDao currencyRateDao);
}
