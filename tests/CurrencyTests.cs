using core;

namespace tests;

public class CurrencyTests
{
    [Fact]
    public void USDComparable_ToUSD() {
        var usd1 = new USD();
        var usd2 = new USD();
        
        Assert.Equal(usd1, usd2);
    }

    [Fact]
    public void USD_IsPreloaded() {
        Assert.NotNull(Currencies.currencyDict[USD.CurrencyCode]);
    }
}

static public class Currencies
{
    public static readonly Dictionary<string, ICurrency> currencyDict = new()
    {
        {USD.CurrencyCode, new USD()}
    };
}