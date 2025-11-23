using CarCare.Processing.Enums;
using CarCare.Processing.Models.Core;

namespace CarCare.Processing.Interfaces.Service;

internal interface ICurrencyRateService
{
    Task<CurrencyRate> GetAsync(Currency from, Currency to);

    Task<CurrencyRate> GetAsync(Currency from, Currency to, DateOnly date);
}
