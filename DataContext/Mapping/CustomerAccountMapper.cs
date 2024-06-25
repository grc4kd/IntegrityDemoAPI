namespace DataContext.Mapping;

public class CustomerAccountMapper
{
    public static Models.CustomerAccount MapToDataContext(Core.Models.CustomerAccount model)
    {
        return new Models.CustomerAccount()
        {
            AccountStatus = model.AccountStatus.StatusCode.ToString(),
            AccountTypeId = model.AccountTypeId,
            Balance = model.Balance,
            Customer = new Models.Customer(),
            Id = model.Id
        };
    }

    public static Core.Models.CustomerAccount MapToModelContext(Models.CustomerAccount entity)
    {
        return new Core.Models.CustomerAccount(
            id: entity.Id,
            accountTypeId: entity.AccountTypeId,
            accountStatusCode: Enum.Parse<Core.Models.AccountStatusCode>(entity.AccountStatus),
            balance: entity.Balance
        );
    }
}