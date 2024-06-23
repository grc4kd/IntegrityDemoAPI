using System.Collections.ObjectModel;

namespace Core.Models.CurrencyTypes;

public class CurrencyFactory {
    public const string DefaultCurrencyCodeEnvironmentVariableName = "DefaultCurrencyCode";
    public static ReadOnlyDictionary<string, Currency> SupportedCurrencyCodes = 
        new(
            new Dictionary<string, Currency>(
            [
                new KeyValuePair<string, Currency>("USD", new USD())
            ]
        ));

    static CurrencyFactory() {
        var defaultCurrencyKey = Environment.GetEnvironmentVariable(DefaultCurrencyCodeEnvironmentVariableName) 
            ?? throw new InvalidOperationException($"Please set the {DefaultCurrencyCodeEnvironmentVariableName} environment variable.");

        if (!SupportedCurrencyCodes.TryGetValue(defaultCurrencyKey, out Currency? defaultCurrencyValue)) {
            throw new InvalidOperationException($"The default currency code set in environment variables: {defaultCurrencyKey} is not supported.");
        }

        DefaultCurrency = defaultCurrencyValue;
    }

    public static Currency Create() => DefaultCurrency;
    private static readonly Currency DefaultCurrency;
}