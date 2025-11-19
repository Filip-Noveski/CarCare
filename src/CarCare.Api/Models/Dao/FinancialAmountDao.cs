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
    public string Currency { get; set; }

    /// <summary>
    /// Creates a new <see cref="FinancialAmountDao"/> instance.
    /// </summary>
    public FinancialAmountDao()
    {
        Amount = 0;
        Currency = "EUR";
    }

    /// <summary>
    /// Creates a new <see cref="FinancialAmountDao"/> instance.
    /// </summary>
    /// <param name="amount">The amount of money.</param>
    /// <param name="currency">The currency of the amount.</param>
    public FinancialAmountDao(double amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }
}
