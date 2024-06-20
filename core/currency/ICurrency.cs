namespace core;

public interface ICurrency : IEquatable<ICurrency> {
    static string CurrencyCode { get; } = "";

    double MinimumAmount();
}