using CarCare.Processing.Enums;
using CarCare.Processing.Models.Core;

namespace CarCare.Processing.Interfaces.Service;

internal interface IFinanceConversionService
{
    Task<FinancialAmount> ConvertAsync(FinancialAmount amount, Currency targetCurrency);

    Task<FinancialAmount> ConvertAsync(FinancialAmount amount, Currency targetCurrency, DateOnly date);
}
