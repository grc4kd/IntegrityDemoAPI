namespace DataContext.Models;

public record CustomerAccount : IAccount
{
    public Customer Customer { get; set; } = null!;
    public long Id { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal Balance { get; set; }
    public string AccountStatus { get; set; } = "OPEN";
};
