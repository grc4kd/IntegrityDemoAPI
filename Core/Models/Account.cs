namespace Core.Models;

public class Account(long id, decimal balance = 0, AccountStatusCode statusCode = AccountStatusCode.OPEN)
{
    public long Id { get; } = id;
    public decimal Balance { get; private set; } = balance;
    public AccountStatus AccountStatus { get; private set; } = new AccountStatus(statusCode);

    public void CloseAccount() {
        if (AccountStatus.StatusCode == AccountStatusCode.CLOSED) {
            throw new InvalidOperationException("The account has already been closed.");
        }

        if (Balance != 0) {
            throw new InvalidOperationException("The account can only be closed if the balance is exactly 0.");
        }

        AccountStatus.StatusCode = AccountStatusCode.CLOSED;
    }
}
