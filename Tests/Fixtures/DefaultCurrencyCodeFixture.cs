namespace Tests.Fixtures;

public class DefaultCurrencyCodeFixture
{
    public DefaultCurrencyCodeFixture() {
        var defaultCurrencyValue = Environment.GetEnvironmentVariable("DefaultCurrencyCode");
        if (defaultCurrencyValue == null || defaultCurrencyValue != "USD") {
            Environment.SetEnvironmentVariable("DefaultCurrencyCode", "USD");
        }
    }
}