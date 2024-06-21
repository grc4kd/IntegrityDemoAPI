using DataContext.Models;
using DataContext.Responses;
using Microsoft.EntityFrameworkCore;

namespace DataContext.Repositories;

public class AccountRepository
{
    private readonly AccountContext _accountDb;

    public AccountRepository(AccountContext accountDb)
    {
        _accountDb = accountDb;
    }

    public async Task<CustomerAccount> GetAccountAsync(long id)
    {
        var account = await _accountDb.Accounts.FindAsync(id);

        if (account != null)
        {
            return account;
        }

        throw new ArgumentOutOfRangeException(nameof(id), $"{GetType().Name} not found by ID.");
    }

    public async Task<List<CustomerAccount>> GetAccountListAsync()
    {
        var maxAccounts = 100;
        return await _accountDb.Accounts
            .OrderBy(a => a.Id)
            .Take(maxAccounts)
            .ToListAsync();
    }

    public async Task<DepositResponse> MakeDepositAsync(CustomerAccount account, decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), $"Expected deposit amount to be positive or zero.");
        }

        var accountEntity = await _accountDb.Accounts
            .Include(account => account.Customer)
            .Where(account => account.Id == account.Id)
            .SingleAsync();

        if (accountEntity == null) {
            throw new ArgumentException("Customer account could not be found.", nameof(account));
        }

        var customerModel = new Core.Models.Customer(accountEntity.Id, accountEntity.Customer.Name);
        var accountModel = new Core.Models.CustomerAccount(customerModel, accountEntity.OpeningBalance);

        accountModel.MakeDeposit(new(amount));

        accountEntity.Balance = accountModel.Balance;

        await _accountDb.SaveChangesAsync();
        
        return new DepositResponse(accountEntity);
    }
}