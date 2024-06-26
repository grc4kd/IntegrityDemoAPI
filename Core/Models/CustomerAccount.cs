namespace Core.Models;

public sealed class CustomerAccount(int id, int accountTypeId, AccountStatusCode accountStatusCode) : Account(id, 0, accountStatusCode)
{    
    private readonly decimal minimumInitialDeposit = 100;

    public int AccountTypeId { get; private set; } = accountTypeId;

    public CustomerAccount(int id, int accountTypeId, AccountStatusCode accountStatusCode, decimal balance) : this(id, accountTypeId, accountStatusCode)
    {
        AccountCurrency.ValidateAmount(balance);

        Balance = balance;
    }

    public void OpenAccount(Deposit initialDeposit, int accountTypeId)
    {
        AccountCurrency.ValidateAmount(initialDeposit.Amount);

        if (initialDeposit.Amount < 100)
        {
            throw new ArgumentException($"Initial deposit is less than minimum required deposit: {minimumInitialDeposit}.");
        }

        Balance = initialDeposit.Amount;
        AccountTypeId = accountTypeId;
    }

    public void MakeDeposit(Deposit deposit)
    {
        AccountCurrency.ValidateAmount(deposit.Amount);

        if (deposit.Currency != AccountCurrency)
        {
            throw new ArgumentException($"Currency of deposit does not match account currency: {AccountCurrency}", nameof(deposit));
        }

        Balance += deposit.Amount;
    }

    public void MakeWithdrawal(Withdrawal withdrawal)
    {
        AccountCurrency.ValidateAmount(withdrawal.Amount);

        if (withdrawal.Currency != AccountCurrency)
        {
            throw new ArgumentException($"Currency of withdrawal does not match account currency: {AccountCurrency}", nameof(withdrawal));
        }

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
