using CarCare.Processing.Enums;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Models.Core;

namespace CarCare.Processing.Services;

internal class FinanceConversionService : IFinanceConversionService
{
    private readonly ICurrencyRateService _currencyService;

    public FinanceConversionService(ICurrencyRateService currencyService)
    {
        _currencyService = currencyService;
    }

    public async Task<FinancialAmount> ConvertAsync(FinancialAmount amount, Currency targetCurrency)
    {
        CurrencyRate rate = await _currencyService.GetAsync(amount.Currency, targetCurrency);
        return new(amount.Amount * rate.Rate, targetCurrency);
    }

    public async Task<FinancialAmount> ConvertAsync(
        FinancialAmount amount, Currency targetCurrency, DateOnly date)
    {
        CurrencyRate rate = await _currencyService.GetAsync(amount.Currency, targetCurrency, date);
        return new(amount.Amount * rate.Rate, targetCurrency);
    }
}
