using DataContext.Models;

namespace DataContext.Responses;

public record WithdrawalResponse(int CustomerId, int AccountId, decimal Balance, bool Succeeded)
    : IAccountResponse
{
    public WithdrawalResponse(IAccount account) : this(
            account.Customer.Id,
            account.Id,
            account.Balance,
            Succeeded: account.Id > -1 && account.Customer.Id > -1
        )
    { }
}