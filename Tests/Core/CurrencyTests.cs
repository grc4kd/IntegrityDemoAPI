using Core.Currency;

namespace Tests.CoreTest;

public class CurrencyTests
{
    // arrange tested variable for all currency tests
    readonly ICurrency currency = CurrencyFactory.Create();

    [Fact]
    public void USD_IsPreloaded()
    {
        Assert.IsType<USD>(currency);
    }

    [Fact]
    public void CurrencyValue_USD_HasDecimalStruct()
    {
        
        Assert.IsType<decimal>(currency.MinimumDenomination);
    }

    [Fact]
    public void CurrencyValue_USD_HasCorrectDenomination()
    {
        Assert.Equal(0.01m, currency.MinimumDenomination);
    }
}
