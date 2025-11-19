using CarCare.Api.Enums;
using CarCare.Api.Interfaces;
using CarCare.Api.Models.Dao;
using CarCare.Api.Models.Json;
using System.Net.Http.Json;

namespace CarCare.Api.Services;

internal class RatesDbCurrencyRatesService : ICurrencyRatesService
{
    private readonly HttpClient _client;

    public RatesDbCurrencyRatesService(HttpClient client)
    {
        _client = client;
    }

    public async Task<FinancialAmountDao> ConvertAsync(FinancialAmountDao amount, Currency currency)
    {
        string from = $"from={amount.Currency.ToString().ToUpper()}";
        string to = $"to={currency.ToString().ToUpper()}";
        string endpoint = $"/rates?{from}&{to}";

        double rate = await Call(endpoint);
        double newAmount = amount.Amount * rate;
        return new(newAmount, currency);
    }

    public async Task<FinancialAmountDao> ConvertAsync(FinancialAmountDao amount, Currency currency, DateOnly date)
    {
        string from = $"from={amount.Currency.ToString().ToUpper()}";
        string to = $"to={currency.ToString().ToUpper()}";
        string dateP = $"date={date:yyyy-mm-dd}";
        string endpoint = $"/rates?{from}&{to}&{dateP}";

        double rate = await Call(endpoint);
        double newAmount = amount.Amount * rate;
        return new(newAmount, currency);
    }

    private async Task<double> Call(string endpoint)
    {
        RatesDbBodyJson response = await _client.GetFromJsonAsync<RatesDbBodyJson>(endpoint)
            ?? throw new InvalidOperationException("The returned JSON was null");

        return response.Data.Rates.First().Value;   // only one rate should be returned
    }
}
