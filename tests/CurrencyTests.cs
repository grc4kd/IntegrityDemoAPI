using core;

namespace tests;

public class CurrencyTests
{
    [Fact]
    public void USD_IsPreloaded() {
        Assert.NotNull(Currencies.currencyDict[USD.CurrencyCode]);
    }

    [Fact]
    public void CurrencyValue_DecimalStruct() {
        Assert.IsType<decimal>(USD.MinimumDenomination);
    }
}

static public class Currencies
{
    public static readonly Dictionary<string, ICurrency> currencyDict = new()
    {
        {USD.CurrencyCode, new USD()}
    };
}