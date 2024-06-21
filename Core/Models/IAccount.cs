namespace Core.Models;

public interface IAccount
{
    public long Id { get; }
    public void MakeDeposit(Deposit deposit);

    public decimal GetBalance();
}
