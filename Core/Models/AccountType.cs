namespace Core.Models;

public static class AccountType
{
    public static int AccountTypeId(string accountTypeName) => accountTypeName switch
    {
        "Checking" => 1,
        "Savings" => 2,
        _ => throw new ArgumentOutOfRangeException(nameof(accountTypeName), $"Unrecognized account type: {accountTypeName}.")
    };

    public static string AccountTypeName(int accountTypeCode) => accountTypeCode switch
    {
        1 => "Checking",
        2 => "Savings",
        _ => "Checking",
    };
    
}