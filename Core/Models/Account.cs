using Core.Models.CurrencyTypes;

namespace Core.Models;

public class Account
{
    public int Id { get; }
    public decimal Balance { get; protected set; }
    public AccountStatus AccountStatus { get; private set; }
    public Currency AccountCurrency { get; private set; }

    public Account(int id, decimal balance = 0m, AccountStatusCode statusCode = AccountStatusCode.OPEN)
    {
        Id = id;
        Balance = balance;
        AccountStatus = new AccountStatus(statusCode);
        AccountCurrency = CurrencyFactory.Create();

        ValidateConstructorParameters();
    }

    private void ValidateConstructorParameters()
    {
        var currency = CurrencyFactory.Create();

        if (Id < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(Id), "Negative account IDs are not allowed.");
        }

        if (Balance < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(Balance), $"{nameof(Balance)} cannot be negative. Value: {Balance}");
        }

        if (Balance.Scale > currency.MinimumDenomination.Scale)
        {
            throw new ArgumentOutOfRangeException(nameof(Balance), $"Balance amount: {Balance} has fractional value less than the minimum denomination for currency code: {currency.CurrencyCode}.");
        }

        if (Balance % currency.MinimumDenomination > 0)
        {
            throw new ArgumentOutOfRangeException(nameof(Balance), $"Balance amount: {Balance} cannot be broken down into the minimum denomination: {currency.MinimumDenomination}.");
        }
    }

    public void CloseAccount()
    {
        if (AccountStatus.StatusCode == AccountStatusCode.CLOSED)
        {
            throw new InvalidOperationException("The account has already been closed.");
        }

        if (Balance != 0)
        {
            throw new InvalidOperationException("The account can only be closed if the balance is exactly 0.");
        }

        AccountStatus.StatusCode = AccountStatusCode.CLOSED;
    }
}
