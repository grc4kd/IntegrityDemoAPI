namespace DataContext.Models;

using AccountStatusCode = Core.Models.AccountStatusCode;
using AccountType = Core.Models.AccountType;

public class CustomerAccount : IAccount
{
    public Customer Customer { get; set; } = null!;
    public long Id { get; set; }
    public decimal Balance { get; set; }
    public string AccountStatus { get; set; } = AccountStatusCode.OPEN.ToString();
    public int AccountTypeId { get; set; } = AccountType.AccountTypeId("Checking");
};
