using Core.Models.CurrencyTypes;

namespace Tests.CoreTests;

public class CurrencyTests
{
    // arrange tested variable for all currency tests
    readonly Currency currency = CurrencyFactory.Create();

    public CurrencyTests() {
        Environment.SetEnvironmentVariable("DefaultCurrencyCode", "USD");
    }

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

    [Fact]
    public void ValidateFractionOfPennies_USD_CheckException() 
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => currency.ValidateAmount(0.001m));
    }

    [Fact]
    public void ValidateFractionofHalfPence_GBP_CheckException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new GBP().ValidateAmount(0.002m));
    }
}

internal record GBP() : Currency("GBP", 0.005m);