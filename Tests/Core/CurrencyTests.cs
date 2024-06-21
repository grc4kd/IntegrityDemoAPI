using Core.Currency;

namespace Tests.Core;

public class CurrencyTests
{
    [Fact]
    public void USD_IsPreloaded()
    {
        var currency = Currencies.GetCurrency("USD");

        Assert.NotNull(currency);
    }

    [Fact]
    public void CurrencyValue_USD_HasDecimalStruct()
    {
        var currency = Currencies.GetCurrency("USD");

        Assert.IsType<decimal>(currency.MinimumDenomination);
    }

    [Fact]
    public void CurrencyValue_USD_HasCorrectDenomination()
    {
        var currency = Currencies.GetCurrency("USD");

        Assert.Equal(0.01m, currency.MinimumDenomination);
        Assert.Equal(0.01f, (float)currency.MinimumDenomination);
        Assert.Equal(0.01d, (double)currency.MinimumDenomination);
    }
}
