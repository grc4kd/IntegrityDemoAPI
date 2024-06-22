namespace DataContext.Models;

public interface IAccount
{
    public Customer Customer { get; set; }
    public long Id { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal Balance { get; set; }
}