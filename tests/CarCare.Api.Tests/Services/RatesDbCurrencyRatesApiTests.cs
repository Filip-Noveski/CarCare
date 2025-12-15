using CarCare.Api.MockApi.Models;
using CarCare.Api.Models.Dao;
using CarCare.Api.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text;

namespace CarCare.Api.Tests.Services;

[Collection("ApiTests")]
public class RatesDbCurrencyRatesApiTests : IClassFixture<WebApplicationFactory<MockApi.Program>>
{
    private const string MockConfigEndpoint = "/RatesMockConfig";
    private const string RequestsEndpoint = "/Requests";

    private readonly HttpClient _client;
    private readonly RatesDbCurrencyRatesApi _sut;

    public RatesDbCurrencyRatesApiTests(WebApplicationFactory<MockApi.Program> factory)
    {
        _client = factory.CreateClient();
        _sut = new(_client);
    }

    [Fact]
    public async Task ShouldConvert()
    {
        // Arrange
        string json = $$"""
        {
            "data": {
                "date": "{{DateTime.Now.Year}}-{{DateTime.Now.Month}}-{{DateTime.Now.Day}}",
                "from": "GBP",
                "rates": {
                    "EUR": 1.20
                }
            }
        }
        """;
        await _client.PostAsync(
            MockConfigEndpoint + "?status=200",
            new StringContent(json, Encoding.UTF8, "application/json"));

        // Act
        CurrencyRateApiModel result = await _sut.GetAsync("GBP", "EUR");

        // Assert
        result.Should().Match<CurrencyRateApiModel>(
            r => r.FromCurrency == "GBP"
            &&  r.ToCurrency == "EUR"
            &&  r.Rate == 1.20
            &&  r.Date == new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));

        Request? request = await _client.GetFromJsonAsync<Request>(RequestsEndpoint);
        request.Should().NotBeNull()
            .And.Match<Request>(r => r.Endpoint == "/rates");
        request.QueryParameters.Should().HaveCount(2);
        request.QueryParameters.Should().ContainKey("from").WhoseValue.Should().Be("GBP");
        request.QueryParameters.Should().ContainKey("to").WhoseValue.Should().Be("EUR");
    }

    [Fact]
    public async Task ShouldConvertForDate()
    {
        // Arrange
        string json = $$"""
        {
            "data": {
                "date": "2023-01-15",
                "from": "USD",
                "rates": {
                    "CAD": 1.35
                }
            }
        }
        """;
        await _client.PostAsync(
            MockConfigEndpoint + "?status=200",
            new StringContent(json, Encoding.UTF8, "application/json"));

        // Act
        CurrencyRateApiModel result = await _sut.GetAsync("USD", "CAD", new DateOnly(2023, 1, 15));

        // Assert
        result.Should().Match<CurrencyRateApiModel>(
            r => r.FromCurrency == "USD"
            &&  r.ToCurrency == "CAD"
            &&  r.Rate == 1.35
            &&  r.Date == new DateOnly(2023, 1, 15));

        Request? request = await _client.GetFromJsonAsync<Request>(RequestsEndpoint);
        request.Should().NotBeNull()
            .And.Match<Request>(r => r.Endpoint == "/rates");
        request.QueryParameters.Should().HaveCount(3);
        request.QueryParameters.Should().ContainKey("from").WhoseValue.Should().Be("USD");
        request.QueryParameters.Should().ContainKey("to").WhoseValue.Should().Be("CAD");
        request.QueryParameters.Should().ContainKey("date").WhoseValue.Should().Be("2023-01-15");
    }
}
