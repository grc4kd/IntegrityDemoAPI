using DataContext.Models;

namespace DataContext.Responses;

public record DepositResponse(long CustomerId, long AccountId, decimal Balance, bool Succeeded)
    : IAccountResponse
{
    public DepositResponse(IAccount account) : this(
        account.Customer.Id,
        account.Id,
        account.Balance,
        Succeeded: account.Id > -1 && account.Customer.Id > -1
    )
    { }
}