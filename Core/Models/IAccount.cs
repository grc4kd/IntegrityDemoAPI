namespace Core.Models;

public interface IAccount
{
    public long Id { get; }
    public long CustomerId { get; }
    public decimal Balance { get; }

    public void MakeDeposit(Deposit deposit);
}
