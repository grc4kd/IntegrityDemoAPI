using DataContext.Models;

namespace DataContext.Responses;

public record CloseAccountResponse(long CustomerId, long AccountId, bool Succeeded)
    : IAccountResponse
{
    public CloseAccountResponse(CustomerAccount account) : this(
            account.Customer.Id,
            account.Id,
            Succeeded: account.Id > -1 && account.Customer.Id > -1
        )
    { }
}