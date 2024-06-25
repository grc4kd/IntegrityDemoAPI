namespace DataContext.Models;

public interface IAccount
{
    public Customer Customer { get; set; }
    public int Id { get; set; }
    public decimal Balance { get; set; }
}