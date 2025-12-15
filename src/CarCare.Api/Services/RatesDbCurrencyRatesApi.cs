using CarCare.Api.Interfaces;
using CarCare.Api.Models.Dao;
using CarCare.Api.Models.Json;
using System.Net.Http.Json;

namespace CarCare.Api.Services;

internal class RatesDbCurrencyRatesApi : ICurrencyRatesApi
{
    private readonly HttpClient _client;

    public RatesDbCurrencyRatesApi(HttpClient client)
    {
        _client = client;
    }

    private async Task<double> Call(string endpoint)
    {
        RatesDbBodyJson response = await _client.GetFromJsonAsync<RatesDbBodyJson>(endpoint)
            ?? throw new InvalidOperationException("The returned JSON was null");

        return response.Data.Rates.First().Value;   // only one rate should be returned
    }

    public async Task<CurrencyRateApiModel> GetAsync(string fromCurrency, string toCurrency)
    {
        string from = $"from={fromCurrency}";
        string to = $"to={toCurrency}";
        string endpoint = $"/rates?{from}&{to}";

        double rate = await Call(endpoint);
        return new(fromCurrency, toCurrency, rate, new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
    }

    public async Task<CurrencyRateApiModel> GetAsync(string fromCurrency, string toCurrency, DateOnly date)
    {
        string from = $"from={fromCurrency}";
        string to = $"to={toCurrency}";
        string dateP = $"date={date:yyyy-MM-dd}";
        string endpoint = $"/rates?{from}&{to}&{dateP}";

        double rate = await Call(endpoint);
        return new(fromCurrency, toCurrency, rate, date);
    }
}
