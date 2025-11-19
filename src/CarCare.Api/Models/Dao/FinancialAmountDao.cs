using CarCare.Api.Enums;

namespace CarCare.Api.Models.Dao;

/// <summary>
/// Represents an amount of money of a currency.
/// </summary>
public class FinancialAmountDao
{
    /// <summary>
    /// The amount of money.
    /// </summary>
    public double Amount { get; set; }

    /// <summary>
    /// The currency.
    /// </summary>
    public Currency Currency { get; set; }

    /// <summary>
    /// Creates a new <see cref="FinancialAmountDao"/> instance.
    /// </summary>
    public FinancialAmountDao()
    {
        Amount = 0;
        Currency = Currency.Eur;
    }

    /// <summary>
    /// Creates a new <see cref="FinancialAmountDao"/> instance.
    /// </summary>
    /// <param name="amount">The amount of money.</param>
    /// <param name="currency">The currency of the amount.</param>
    public FinancialAmountDao(double amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }
}
