using CarCare.Api.Models.Dao;

namespace CarCare.Api.Interfaces;

/// <summary>
/// Manages exchange rates between currencies.
/// </summary>
public interface ICurrencyRatesService
{
    /// <summary>
    /// Converts the <paramref name="amount"/> to the provided <paramref name="currency"/>.
    /// </summary>
    /// <param name="amount">The <see cref="FinancialAmountDao"/> to convert.</param>
    /// <param name="currency">The currency to convert to.</param>
    /// <returns>A new <see cref="FinancialAmountDao"/> object.</returns>
    Task<FinancialAmountDao> ConvertAsync(FinancialAmountDao amount, string currency);

    /// <summary>
    /// Converts the <paramref name="amount"/> to the provided <paramref name="currency"/>
    /// with rates for the provided <paramref name="date"/>.
    /// </summary>
    /// <param name="amount">The <see cref="FinancialAmountDao"/> to convert.</param>
    /// <param name="currency">The currency to convert to.</param>
    /// <param name="date">The <see cref="DateOnly"/> to fetch rates for.</param>
    /// <returns>A new <see cref="FinancialAmountDao"/> object.</returns>
    Task<FinancialAmountDao> ConvertAsync(FinancialAmountDao amount, string currency, DateOnly date);
}
