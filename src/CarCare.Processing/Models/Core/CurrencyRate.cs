using CarCare.Persistence.Models;
using CarCare.Processing.Enums;

namespace CarCare.Processing.Models.Core;

internal class CurrencyRate
{
    public Currency FromCurrency { get; set; }

    public Currency ToCurrency { get; set; }

    public double Rate { get; set; }

    public DateOnly Date { get; set; }

    public CurrencyRate(
        Currency fromCurrency, Currency toCurrency, double rate, DateOnly date)
    {
        FromCurrency = fromCurrency;
        ToCurrency = toCurrency;
        Rate = rate;
        Date = date;
    }

    public CurrencyRateDao ToDao()
    {
        return new CurrencyRateDao(
            FromCurrency.ToString().ToUpper(),
            ToCurrency.ToString().ToUpper(),
            Rate,
            Date);
    }
}
