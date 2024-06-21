namespace Core.Currency;

public static class CurrencyFactory {
    public static ICurrency Create() {
        return DefaultCurrency;
    }

    private static ICurrency DefaultCurrency => Currencies.GetCurrency(USD.s_CurrencyCode);
}