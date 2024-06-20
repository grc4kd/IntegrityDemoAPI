using core;
using core.currency;

namespace tests;

public class CurrencyTests
{
    [Fact]
    public void USD_IsPreloaded() {
        var currency = Currencies.GetCurrency("USD");

        Assert.NotNull(currency);
    }

    [Fact]
    public void CurrencyValue_USD_HasDecimalStruct() {
        var currency = Currencies.GetCurrency("USD");

        Assert.IsType<decimal>(currency.MinimumDenomination);
    }
}
