using Core.CQRS.Response;
using Core.Models;

namespace Core.Repositories;

public class AccountRepository(IAccountSet accountSet) : IAccountRepository
{
    private readonly IAccountSet accountSet = accountSet;

    public CustomerAccount GetAccount(long id)
    {
        return accountSet.GetAccount(id);
    }

    public DepositResponse MakeDeposit(IAccount account, decimal amount)
    {
        if (amount < 0) {
            throw new ArgumentOutOfRangeException(nameof(amount), $"Expected deposit amount to be positive or zero.");
        }

        account.MakeDeposit(new(amount));
        
        DepositResponse response = new(account.GetBalance());

        return response;
    }
}