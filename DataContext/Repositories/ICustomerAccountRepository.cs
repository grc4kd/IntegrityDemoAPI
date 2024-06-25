using DataContext.Models;
using DataContext.Requests;
using DataContext.Responses;

namespace DataContext.Repositories;

public interface ICustomerAccountRepository
{
    public CustomerAccount? GetCustomerAccount(int accountId);
    public IEnumerable<CustomerAccount> GetCustomerAccounts(int maxLength);
    public DepositResponse MakeDeposit(DepositRequest request);
    public WithdrawalResponse MakeWithdrawal(WithdrawalRequest request);
    public OpenAccountResponse OpenCustomerAccount(OpenAccountRequest request);
    public CloseAccountResponse CloseCustomerAccount(CloseAccountRequest request);
}