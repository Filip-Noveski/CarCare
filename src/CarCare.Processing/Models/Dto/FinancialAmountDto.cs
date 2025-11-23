using CarCare.Processing.Enums;

namespace CarCare.Processing.Models.Dto;

/// <summary>
/// An amount of a given currency.
/// </summary>
public class FinancialAmountDto
{
    /// <summary>
    /// The amount of the given currency.
    /// </summary>
    public double Amount { get; set; }

    /// <summary>
    /// The currency of the amount.
    /// </summary>
    public Currency Currency { get; set; }

    /// <summary>
    /// Creates a new instance of the <see cref="FinancialAmountDto"/> class.
    /// </summary>
    /// <param name="amount">The amount of the currency.</param>
    /// <param name="currency">The currency.</param>
    public FinancialAmountDto(double amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }
}
