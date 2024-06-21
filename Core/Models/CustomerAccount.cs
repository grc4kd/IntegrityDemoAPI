namespace Core.Models;

public class CustomerAccount : IAccount
{
    public long Id { get; }
    public Customer Customer { get; }
    public long CustomerId { get; }
    public decimal Balance { get; private set; }

    public CustomerAccount(Customer customer, decimal openingBalance = 0)
    {
        Customer = customer;

        if (ValidateConstructorParameters(openingBalance))
        {
            Balance = openingBalance;
        }
    }

    public void MakeDeposit(Deposit deposit)
    {
        if (deposit.Currency is Currency.USD)
        {
            Balance += deposit.Amount;
        }
    }

    private static bool ValidateConstructorParameters(decimal openingBalance)
    {
        if (openingBalance < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(openingBalance),
                $"{nameof(openingBalance)} cannot be negative. Value: {openingBalance}");
        }

        return true;
    }
}