namespace DataContext.Requests;

public class WithdrawalRequest
{
    public long CustomerId { get; set; }
    public long AccountId { get; set; }
    public double Amount { get; set; }
}