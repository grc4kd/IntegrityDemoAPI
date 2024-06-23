namespace Core.Models;

public class CustomerAccount
{
    public long Id { get; }
    public Customer Customer { get; }
    public long CustomerId { get; }
    public decimal Balance { get; private set; }
    public AccountStatus AccountStatus { get; private set; }

    public CustomerAccount(Customer customer, decimal openingBalance = 0)
    {
        Customer = customer;
        AccountStatus = new AccountStatus();

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

    public void MakeWithdrawal(Withdrawal withdrawal)
    {
        if (withdrawal.Currency is Currency.USD)
        {
            var remainingBalance = Balance - withdrawal.Amount;

            if (remainingBalance < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(withdrawal), "Insufficient funds to make withdrawal.");
            }
            if (remainingBalance >= 0)
            {
                Balance = remainingBalance;
            }
        }
    }

    public void CloseAccount() {
        if (AccountStatus.StatusCode == AccountStatusCode.CLOSED) {
            throw new InvalidOperationException("The account has already been closed.");
        }

        if (Balance != 0) {
            throw new InvalidOperationException("The account can only be closed if the balance is exactly 0.");
        }

        AccountStatus.StatusCode = AccountStatusCode.CLOSED;
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
