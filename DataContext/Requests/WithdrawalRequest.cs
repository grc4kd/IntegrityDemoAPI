namespace DataContext.Requests;

public record WithdrawalRequest
{
    public long CustomerId { get; set; }
    public long AccountId { get; set; }
    public double Amount { get; set; }
}