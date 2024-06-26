using DataContext.Models;

namespace DataContext.Responses;

public record OpenAccountResponse(int CustomerId, int AccountId, bool Succeeded, int AccountTypeId, decimal Balance)
    : IAccountResponse
{
    public OpenAccountResponse(CustomerAccount account) : this(
            account.Customer.Id,
            account.Id,
            Succeeded: account.Id > -1 && account.Customer.Id > -1,
            account.AccountTypeId,
            account.Balance
        )
    { }
}