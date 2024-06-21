using DataContext.Models;

namespace DataContext.Responses;

public class DepositResponse(long customerId, long accountId, decimal balance, bool succeeded)
{
    public long CustomerId { get; } = customerId;
    public long AccountId { get; } = accountId;
    public decimal Balance { get; } = balance;
    public bool Succeeded { get; } = succeeded;

    public DepositResponse(CustomerAccount customerAccount) : this(
        customerAccount.Customer.Id,
        customerAccount.Id,
        customerAccount.Balance,
        succeeded: customerAccount.Id > -1 && customerAccount.Customer.Id > -1)
    { }
}