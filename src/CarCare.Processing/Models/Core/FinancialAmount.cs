using CarCare.Processing.Enums;

namespace CarCare.Processing.Models.Core;

internal class FinancialAmount
{
    public double Amount { get; set; }

    public Currency Currency { get; set; }

    public FinancialAmount(double amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }
}
