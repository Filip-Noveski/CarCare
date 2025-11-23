namespace CarCare.Api.Models.Dao;

/// <summary>
/// A conversion rate between two currencies.
/// </summary>
public class CurrencyRateApiModel
{
    /// <summary>
    /// The source currency.
    /// </summary>
    public string FromCurrency { get; set; }
    
    /// <summary>
    /// The target currency.
    /// </summary>
    public string ToCurrency { get; set; }

    /// <summary>
    /// The rate between the currencies.
    /// </summary>
    public double Rate { get; set; }

    /// <summary>
    /// The date for which the rate is valid.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Creates a new <see cref="CurrencyRateApiModel"/> instance.
    /// </summary>
    public CurrencyRateApiModel()
    {
        FromCurrency = "EUR";
        ToCurrency = "EUR";
        Rate = 1.0;
        Date = new(2025, 11, 23);
    }

    /// <summary>
    /// Creates a new <see cref="CurrencyRateApiModel"/> instance.
    /// </summary>
    /// <param name="fromCurrency">The source currency.</param>
    /// <param name="toCurrency">The target currency.</param>
    /// <param name="rate">The rate between the currencies.</param>
    /// <param name="date">The date of the rate.</param>
    public CurrencyRateApiModel(string fromCurrency, string toCurrency, double rate, DateOnly date)
    {
        FromCurrency = fromCurrency;
        ToCurrency = toCurrency;
        Rate = rate;
        Date = date;
    }
}
