namespace Core.Models;

public sealed class CustomerAccount(long id = 0, decimal openingBalance = 0) : Account(id, openingBalance, AccountStatusCode.OPEN)
{
    public void MakeDeposit(Deposit deposit)
    {
        if (deposit.Currency != AccountCurrency) {
            throw new ArgumentException($"Currency of deposit does not match account currency: {AccountCurrency}", nameof(deposit));
        }

        Balance += deposit.Amount;
    }

    public void MakeWithdrawal(Withdrawal withdrawal)
    {
        if (withdrawal.Currency != AccountCurrency) {
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
