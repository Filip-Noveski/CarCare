namespace CarCare.Persistence.Models;

/// <summary>
/// A database currency rate cache model.
/// </summary>
public class CurrencyRateDao
{
    /// <summary>
    /// The source currency of the rate.
    /// </summary>
    public string FromCurrency { get; set; }
    
    /// <summary>
    /// The target currency of the rate.
    /// </summary>
    public string ToCurrency { get; set; }

    /// <summary>
    /// The rate when converting from the source to the target currency.
    /// </summary>
    public double Rate { get; set; }

    /// <summary>
    /// The date for which this conversion rate is valid.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Creates a new <see cref="CurrencyRateDao"/> object.
    /// </summary>
    public CurrencyRateDao()
    {
        FromCurrency = string.Empty;
        ToCurrency = string.Empty;
        Rate = 0.0;
        Date = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
    }

    /// <summary>
    /// Creates a new <see cref="CurrencyRateDao"/> object.
    /// </summary>
    /// <param name="fromCurrency">The source currency.</param>
    /// <param name="toCurrency">The target currency.</param>
    /// <param name="rate">The rate between the currencies.</param>
    /// <param name="date">The date for which the rate is valid.</param>
    public CurrencyRateDao(string fromCurrency, string toCurrency, double rate, DateOnly date)
    {
        FromCurrency = fromCurrency;
        ToCurrency = toCurrency;
        Rate = rate;
        Date = date;
    }
}
