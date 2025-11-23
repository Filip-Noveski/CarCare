using CarCare.Api.Interfaces;
using CarCare.Api.Models.Dao;
using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using CarCare.Processing.Enums;
using CarCare.Processing.Models.Core;
using CarCare.Processing.Services;
using FluentAssertions;
using NSubstitute;

namespace CarCare.Processing.Tests.Services;

public class CurrencyRatesServiceTests
{
    private readonly ICurrencyRatesCacheRepository _currencyRepository;
    private readonly ICurrencyRatesApi _currencyApi;
    private readonly CurrencyRateService _sut;

    public CurrencyRatesServiceTests()
    {
        _currencyRepository = Substitute.For<ICurrencyRatesCacheRepository>();
        _currencyApi = Substitute.For<ICurrencyRatesApi>();
        _sut = new(_currencyRepository, _currencyApi);
    }

    [Fact]
    public async Task ShouldGetResultForTodayFromRepository()
    {
        // Arrange
        Currency fromC = Currency.Eur;
        Currency toC = Currency.Aud;
        DateOnly date = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        CurrencyRateDao repoResult = new("EUR", "AUD", 1.5, date);
        _currencyRepository.GetIfExistsAsync("EUR", "AUD", date).Returns(repoResult);

        // Act
        CurrencyRate result = await _sut.GetAsync(fromC, toC);

        // Assert
        await _currencyRepository.Received().GetIfExistsAsync("EUR", "AUD", date);
        await _currencyApi.DidNotReceive().GetAsync(Arg.Any<string>(), Arg.Any<string>());
        await _currencyApi.DidNotReceive().GetAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateOnly>());

        result.FromCurrency.Should().Be(fromC);
        result.ToCurrency.Should().Be(toC);
        result.Rate.Should().Be(1.5);
        result.Date.Should().Be(date);
    }

    [Fact]
    public async Task ShouldGetResultForTodayFromApi()
    {
        // Arrange
        Currency fromC = Currency.Eur;
        Currency toC = Currency.Aud;
        DateOnly date = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        _currencyRepository.GetIfExistsAsync("EUR", "AUD", date).Returns((CurrencyRateDao?)null);
        CurrencyRateApiModel apiResult = new("EUR", "AUD", 1.5, date);
        _currencyApi.GetAsync("EUR", "AUD").Returns(apiResult);

        // Act
        CurrencyRate result = await _sut.GetAsync(fromC, toC);

        // Assert
        await _currencyRepository.Received().GetIfExistsAsync("EUR", "AUD", date);
        await _currencyApi.Received().GetAsync("EUR", "AUD");
        await _currencyApi.DidNotReceive().GetAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateOnly>());

        result.FromCurrency.Should().Be(fromC);
        result.ToCurrency.Should().Be(toC);
        result.Rate.Should().Be(1.5);
        result.Date.Should().Be(date);
    }

    [Fact]
    public async Task ShouldGetResultForEarlierDayFromRepository()
    {
        // Arrange
        Currency fromC = Currency.Eur;
        Currency toC = Currency.Aud;
        DateOnly date = new(2025, 8, 16);
        CurrencyRateDao repoResult = new("EUR", "AUD", 1.5, date);
        _currencyRepository.GetIfExistsAsync("EUR", "AUD", date).Returns(repoResult);

        // Act
        CurrencyRate result = await _sut.GetAsync(fromC, toC, date);

        // Assert
        await _currencyRepository.Received().GetIfExistsAsync("EUR", "AUD", date);
        await _currencyApi.DidNotReceive().GetAsync(Arg.Any<string>(), Arg.Any<string>());
        await _currencyApi.DidNotReceive().GetAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateOnly>());

        result.FromCurrency.Should().Be(fromC);
        result.ToCurrency.Should().Be(toC);
        result.Rate.Should().Be(1.5);
        result.Date.Should().Be(date);
    }

    [Fact]
    public async Task ShouldGetResultForEarlierDayFromApi()
    {
        // Arrange
        Currency fromC = Currency.Eur;
        Currency toC = Currency.Aud;
        DateOnly date = new(2025, 10, 12);
        _currencyRepository.GetIfExistsAsync("EUR", "AUD", date).Returns((CurrencyRateDao?)null);
        CurrencyRateApiModel apiResult = new("EUR", "AUD", 1.5, date);
        _currencyApi.GetAsync("EUR", "AUD", date).Returns(apiResult);

        // Act
        CurrencyRate result = await _sut.GetAsync(fromC, toC, date);

        // Assert
        await _currencyRepository.Received().GetIfExistsAsync("EUR", "AUD", date);
        await _currencyApi.DidNotReceive().GetAsync(Arg.Any<string>(), Arg.Any<string>());
        await _currencyApi.Received().GetAsync("EUR", "AUD", date);

        result.FromCurrency.Should().Be(fromC);
        result.ToCurrency.Should().Be(toC);
        result.Rate.Should().Be(1.5);
        result.Date.Should().Be(date);
    }
}
