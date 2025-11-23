using CarCare.Processing.Enums;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Models.Core;
using CarCare.Processing.Services;
using FluentAssertions;
using NSubstitute;

namespace CarCare.Processing.Tests.Services;

public class FinanceConversionServiceTests
{
    private readonly ICurrencyRateService _currencyService;
    private readonly FinanceConversionService _sut;

    public FinanceConversionServiceTests()
    {
        _currencyService = Substitute.For<ICurrencyRateService>();
        _sut = new(_currencyService);
    }

    [Fact]
    public async Task ShouldConvertForToday()
    {
        // Arrange
        CurrencyRate rate = new(
            Currency.Eur, Currency.Aud, 1.5, new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
        _currencyService.GetAsync(Currency.Eur, Currency.Aud).Returns(rate);
        FinancialAmount amount = new(1_000, Currency.Eur);
        Currency target = Currency.Aud;

        // Act
        FinancialAmount result = await _sut.ConvertAsync(amount, target);

        // Assert
        await _currencyService.Received().GetAsync(Currency.Eur, Currency.Aud);
        result.Amount.Should().Be(1_500);
        result.Currency.Should().Be(target);
    }

    [Fact]
    public async Task ShouldConvertForEarlierDay()
    {
        // Arrange
        DateOnly date = new(2024, 12, 18);
        CurrencyRate rate = new(Currency.Eur, Currency.Aud, 1.5, date);
        _currencyService.GetAsync(Currency.Eur, Currency.Aud, date).Returns(rate);
        FinancialAmount amount = new(1_000, Currency.Eur);
        Currency target = Currency.Aud;

        // Act
        FinancialAmount result = await _sut.ConvertAsync(amount, target, date);

        // Assert
        await _currencyService.Received().GetAsync(Currency.Eur, Currency.Aud, date);
        result.Amount.Should().Be(1_500);
        result.Currency.Should().Be(target);
    }
}
