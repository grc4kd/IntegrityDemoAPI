using Core.CQRS.Response;
using Core.Models;

namespace Core.Repositories;

public interface IAccountRepository {
    CustomerAccount GetAccount(long id);
    DepositResponse MakeDeposit(IAccount account, decimal amount);
}