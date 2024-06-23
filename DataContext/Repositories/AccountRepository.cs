using DataContext.Models;
using DataContext.Responses;
using Microsoft.EntityFrameworkCore;

namespace DataContext.Repositories;

public class AccountRepository(DbContextOptions<AccountContext> options)
{
    private readonly DbContextOptions<AccountContext> _options = options;

    public void AddAccount(CustomerAccount customerAccount)
    {
        // set customer account balance to opening balance unless a different value has already been set in data model
        if (customerAccount.Balance == 0) {
            customerAccount.Balance = customerAccount.OpeningBalance;
        }

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

    public DepositResponse MakeDeposit(CustomerAccount toAccount, decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), $"Expected deposit amount to be positive or zero.");
        }

        using var context = new AccountContext(_options);
        
        CustomerAccount customerAccount = FindCustomerAccount(toAccount);

        var customer = new Core.Models.Customer(customerAccount.Id, toAccount.Customer.Name);
        var account = new Core.Models.CustomerAccount(customerAccount.Customer.Id, toAccount.Balance);

        account.MakeDeposit(new(amount));

        customerAccount.Balance = account.Balance;

        context.SaveChanges();

        return new DepositResponse(customerAccount);
    }

    public WithdrawalResponse MakeWithdrawal(CustomerAccount fromAccount, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), $"Expected withdrawal amount to be greater than zero.");
        }

        using var context = new AccountContext(_options);

        CustomerAccount customerAccount = FindCustomerAccount(fromAccount);

        var customer = new Core.Models.Customer(fromAccount.Id, fromAccount.Customer.Name);
        var account = new Core.Models.CustomerAccount(customerAccount.Customer.Id, fromAccount.Balance);

        
        account.MakeWithdrawal(new(amount));

        customerAccount.Balance = account.Balance;

        context.SaveChanges();

        return new WithdrawalResponse(customerAccount);
    }

    public CloseAccountResponse CloseAccount(long id)
    {
        using var context = new AccountContext(_options);

        var account = context.Accounts
            .Include(a => a.Customer)
            .Where(a => a.Id == id)
            .SingleOrDefault();

        if (account == null)
        {
            throw new ArgumentException("Customer account could not be found.", nameof(id));
        }

        if (!Enum.TryParse<Core.Models.AccountStatusCode>(account.AccountStatus, out var accountStatusCode))
        {
            throw new InvalidOperationException(
                $"Unknown {nameof(Core.Models.AccountStatusCode)} was found: {account.AccountStatus}");
        }

        Core.Models.AccountStatus accountStatusModel = new(accountStatusCode);
        Core.Models.Account accountModel = new(account.Id, account.Balance, accountStatusCode);

        accountModel.CloseAccount();

        if (accountModel.AccountStatus.StatusCode == accountStatusModel.StatusCode)
        {
            throw new InvalidOperationException(
                $"Account status has already been changed from {accountStatusModel.StatusCode} to {accountModel.AccountStatus.StatusCode}");
        }

        account.AccountStatus = Enum.GetName(accountModel.AccountStatus.StatusCode)!;
        
        context.SaveChanges();

        return new CloseAccountResponse(account);
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

    private CustomerAccount FindCustomerAccount(CustomerAccount customerAccount)
    {
        using var context = new AccountContext(_options);

        return context.Accounts
            .Include(a => a.Customer)
            .Where(a => a.Id == customerAccount.Id)
            .SingleOrDefault() ?? throw new ArgumentException("Customer account could not be found.", nameof(customerAccount));
    }
}

