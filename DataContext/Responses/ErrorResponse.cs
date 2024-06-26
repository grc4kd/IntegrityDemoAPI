using DataContext.Models;

namespace DataContext.Responses;

public record ErrorResponse(int CustomerId, int AccountId, string Error = "")
    : IAccountResponse
{
    public bool Succeeded => false;

    public ErrorResponse(CustomerAccount account) : this(
            account.Customer.Id,
            account.Id
        )
    { }
}