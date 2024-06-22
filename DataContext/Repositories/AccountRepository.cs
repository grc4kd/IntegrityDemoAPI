using DataContext.Models;
using DataContext.Responses;
using Microsoft.EntityFrameworkCore;

namespace DataContext.Repositories;

public class AccountRepository
{
    private readonly DbContextOptions<AccountContext> _options;

    public AccountRepository(DbContextOptions<AccountContext> options)
    {
        _options = options;
    }

    public void AddAccount(CustomerAccount customerAccount)
    {
        using var context = new AccountContext(_options);
        
        context.Accounts.Add(customerAccount);
        context.SaveChanges();
    }

    public bool DeleteAccount(long id)
    {
        using var context = new AccountContext(_options);

        var customerAccount = context.Accounts.Find(id);
        if (customerAccount == null)
        {
            return false;
        }

        context.Accounts.Remove(customerAccount);
        context.SaveChanges();

        return true;
    }

    public CustomerAccount? GetAccount(long id)
    {
        using var context = new AccountContext(_options);
        return context.Accounts
            .Include(a => a.Customer)
            .SingleOrDefault(a => a.Id == id);
    }

    public List<CustomerAccount> GetAccountList()
    {
        using var context = new AccountContext(_options);

        var maxAccounts = 100;
        return context.Accounts
            .Include(a => a.Customer)
            .OrderBy(a => a.Id)
            .Take(maxAccounts)
            .ToList();
    }

    public DepositResponse MakeDeposit(CustomerAccount account, decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), $"Expected deposit amount to be positive or zero.");
        }

        using var context = new AccountContext(_options);

        var accountEntity = context.Accounts
            .Include(a => a.Customer)
            .Where(a => a.Id == account.Id)
            .SingleOrDefault();

        if (accountEntity == null) {
            throw new ArgumentException("Customer account could not be found.", nameof(account));
        }

        var customerModel = new Core.Models.Customer(accountEntity.Id, accountEntity.Customer.Name);
        var accountModel = new Core.Models.CustomerAccount(customerModel, accountEntity.Balance);

        accountModel.MakeDeposit(new(amount));

        accountEntity.Balance = accountModel.Balance;

        context.SaveChanges();
        
        return new DepositResponse(accountEntity);
    }

    public WithdrawalResponse MakeWithdrawal(CustomerAccount account, decimal amount)
    {
        if (amount <= 0) {
            throw new ArgumentOutOfRangeException(nameof(amount), $"Expected withdrawal amount to be greater than zero.");
        }

        using var context = new AccountContext(_options);

        var accountEntity = context.Accounts
            .Include(a => a.Customer)
            .Where(a => a.Id == account.Id)
            .SingleOrDefault();

        if (accountEntity == null) {
            throw new ArgumentException("Customer account could not be found.", nameof(account));
        }

        var customerModel = new Core.Models.Customer(accountEntity.Id, accountEntity.Customer.Name);
        var accountModel = new Core.Models.CustomerAccount(customerModel, accountEntity.Balance);

        accountModel.MakeWithdrawal(new(amount));

        accountEntity.Balance = accountModel.Balance;

        context.SaveChanges();
        
        return new WithdrawalResponse(accountEntity);
    }

    public bool PutCustomerAccount(long id, CustomerAccount customerAccount)
    {
        using var context = new AccountContext(_options);

        context.Entry(customerAccount).State = EntityState.Modified;

        try
        {
            context.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!context.Accounts.Any(e => e.Id == id))
            {
                return false;
            }
            else
            {
                throw;
            }
        }

        return true;
    }
}

