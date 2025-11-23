using CarCare.Api.Interfaces;
using CarCare.Api.Models.Dao;
using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using CarCare.Processing.Enums;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Models.Core;

namespace CarCare.Processing.Services;

internal class CurrencyRateService : ICurrencyRateService
{
    private readonly ICurrencyRatesCacheRepository _currencyRepository;
    private readonly ICurrencyRatesApi _currencyApi;

    public CurrencyRateService(
        ICurrencyRatesCacheRepository currencyRepository, 
        ICurrencyRatesApi currencyApi)
    {
        _currencyRepository = currencyRepository;
        _currencyApi = currencyApi;
    }

    public async Task<CurrencyRate> GetAsync(Currency from, Currency to)
    {
        string fromStr = from.ToString().ToUpper();
        string toStr = to.ToString().ToUpper();
        DateOnly date = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);   // today
        CurrencyRateDao? dao = await _currencyRepository.GetIfExistsAsync(fromStr, toStr, date);

        if (dao is not null)
        {
            return new(from, to, dao.Rate, date);
        }

        CurrencyRateApiModel apiModel = await _currencyApi.GetAsync(fromStr, toStr);
        CurrencyRate rate = new(from, to, apiModel.Rate, apiModel.Date);
        CurrencyRateDao toCache = rate.ToDao();
        await _currencyRepository.AddAsync(toCache);
        return rate;
    }

    public async Task<CurrencyRate> GetAsync(Currency from, Currency to, DateOnly date)
    {
        string fromStr = from.ToString().ToUpper();
        string toStr = to.ToString().ToUpper();
        CurrencyRateDao? dao = await _currencyRepository.GetIfExistsAsync(fromStr, toStr, date);

        if (dao is not null)
        {
            return new(from, to, dao.Rate, date);
        }

        CurrencyRateApiModel apiModel = await _currencyApi.GetAsync(fromStr, toStr, date);
        CurrencyRate rate = new(from, to, apiModel.Rate, date);
        CurrencyRateDao toCache = rate.ToDao();
        await _currencyRepository.AddAsync(toCache);
        return rate;
    }
}
