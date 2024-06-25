using DataContext.Mapping;
using DataContext.Models;
using DataContext.Requests;
using DataContext.Responses;
using Microsoft.EntityFrameworkCore;

namespace DataContext.Repositories;

public class CustomerAccountRepository : ICustomerAccountRepository
{
    private readonly CustomerAccountContext _context;

    public CustomerAccountRepository(CustomerAccountContext context) 
        => _context = context;

    public CustomerAccount? GetCustomerAccount(long id)
    {
        return _context.Accounts
            .Include(a => a.Customer)
            .SingleOrDefault(a => a.Id == id);
    }

    public IEnumerable<CustomerAccount> GetCustomerAccounts(int maxLength)
    {
        return _context.Accounts
            .Include(a => a.Customer)
            .OrderBy(a => a.Id)
            .Take(maxLength);
    }

    public DepositResponse MakeDeposit(DepositRequest request)
    {

        if (request.Amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request),
                $"Expected {nameof(DepositRequest)} {nameof(request.Amount)} to be positive or zero.");
        }

        CustomerAccount customerAccount = FindAccount(request.AccountId);

        var model = CustomerAccountMapper.MapToModelContext(customerAccount);

        model.MakeDeposit(new((decimal)request.Amount));

        customerAccount.Balance = model.Balance;

        _context.Update(customerAccount);
        _context.SaveChanges();

        return new DepositResponse(customerAccount);
    }

    public WithdrawalResponse MakeWithdrawal(WithdrawalRequest request)
    {
        if (request.Amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request),
                $"Expected {nameof(WithdrawalRequest)} {nameof(request.Amount)} to be greater than zero.");
        }

        var customerAccount = FindAccount(request.AccountId);

        var model = CustomerAccountMapper.MapToModelContext(customerAccount);

        model.MakeWithdrawal(new((decimal)request.Amount));

        customerAccount.Balance = model.Balance;

        _context.Update(customerAccount);
        _context.SaveChanges();

        return new WithdrawalResponse(customerAccount);
    }

    public OpenAccountResponse OpenCustomerAccount(OpenAccountRequest request)
    {
        var (customerId, accountId, accountTypeId) = request;

        var customer = _context.Find<Customer>(customerId) ??
            throw new ArgumentException($"Customer ID {customerId} does not exist.", nameof(request));

        if (Core.Models.AccountType.AccountTypeName(accountTypeId) == "Checking" &&
            !_context.Accounts
            .Include(a => a.Customer)
            .Any(a => a.Customer.Id == customerId && a.AccountTypeId == Core.Models.AccountType.AccountTypeId("Savings")))
        {
            throw new InvalidOperationException("A savings account must be created before any checking accounts.");
        }

        var model = new Core.Models.CustomerAccount(
            id: 0,
            accountTypeId: accountTypeId,
            accountStatusCode: Core.Models.AccountStatusCode.OPEN);

        model.OpenAccount(new((decimal)request.InitialDeposit), accountTypeId);

        var entity = CustomerAccountMapper.MapToDataContext(model);
        entity.Customer = customer;

        _context.Update(entity);
        _context.SaveChanges();

        return new OpenAccountResponse(entity);
    }

    public CloseAccountResponse CloseCustomerAccount(CloseAccountRequest request)
    {
        var customerAccount = FindAccount(request.AccountId);

        if (!Enum.TryParse<Core.Models.AccountStatusCode>(customerAccount.AccountStatus, out var accountStatusCode))
        {
            throw new InvalidOperationException(
                $"Unknown {nameof(Core.Models.AccountStatusCode)}: {customerAccount.AccountStatus}");
        }

        var model = CustomerAccountMapper.MapToModelContext(customerAccount);

        model.CloseAccount();

        customerAccount.AccountStatus = model.AccountStatus.StatusCode.ToString();

        _context.SaveChanges();

        return new CloseAccountResponse(customerAccount);
    }

    private CustomerAccount FindAccount(long id)
    {
        return _context.Accounts
            .Include(a => a.Customer)
            .Where(a => a.Id == id)
            .SingleOrDefault() ??
            throw new ArgumentOutOfRangeException(nameof(id), $"Customer account could not be found, ID: {id}.");
    }
}

