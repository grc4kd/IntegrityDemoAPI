using Core.Models.CurrencyTypes;

namespace Tests.CoreTests;

[Collection("Environment collection")]
public class CurrencyTests(EnvironmentFixture fixture)
{
    // test collection fixture loads default currency code for all tests in this class
    readonly EnvironmentFixture fixture = fixture;

    [Fact]
    public void EnvironmentFixture_Default_USD()
    {
        string? defaultCurrencyCode = Environment.GetEnvironmentVariable("DefaultCurrencyCode");

        Assert.NotNull(defaultCurrencyCode);
        Assert.Equal(defaultCurrencyCode, CurrencyFactory.Create().CurrencyCode);
    }

    [Fact]
    public void CurrencyValue_USD_HasDecimalStruct()
    {

        Assert.IsType<decimal>(new USD().MinimumDenomination);
    }

    [Fact]
    public void CurrencyValue_USD_HasCorrectDenomination()
    {
        Assert.Equal(0.01m, new USD().MinimumDenomination);
    }

    [Theory(DisplayName = "FractionalAmountTest")]
    [InlineData("CLP", 0, .1)]
    [InlineData("JOD", 0.001, .0001)]
    [InlineData("USD", 0.01, .001)]
    public void ValidateFractionofUnit_CheckException(string currencyCode, decimal minimumDenomination, decimal amount)
    {
        Currency currency = new(currencyCode, minimumDenomination);
        Assert.Throws<ArgumentOutOfRangeException>(() => currency.ValidateAmount(amount));
    }
}
