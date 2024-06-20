namespace core;

public interface ICurrency : IEquatable<ICurrency> {
    static string CurrencyCode { get; } = string.Empty;

    decimal MinimumDenomination();
}