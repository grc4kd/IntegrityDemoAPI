namespace Core.Models;

public class AccountStatus(AccountStatusCode StatusCode = AccountStatusCode.OPEN)
{
    public AccountStatusCode StatusCode = StatusCode;
}
