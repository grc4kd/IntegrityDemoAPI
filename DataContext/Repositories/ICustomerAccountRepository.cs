using DataContext.Models;
using DataContext.Requests;
using DataContext.Responses;

namespace DataContext.Repositories;

public interface ICustomerAccountRepository
{
    public CustomerAccount? GetCustomerAccount(int accountId);
    public IEnumerable<CustomerAccount> GetCustomerAccounts(int maxLength);
    public IAccountResponse MakeDeposit(DepositRequest request);
    public IAccountResponse MakeWithdrawal(WithdrawalRequest request);
    public OpenAccountResponse OpenCustomerAccount(OpenAccountRequest request);
    public CloseAccountResponse CloseCustomerAccount(CloseAccountRequest request);
}