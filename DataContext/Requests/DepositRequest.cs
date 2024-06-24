namespace DataContext.Requests;

public record struct DepositRequest
{
    public long CustomerId { get; set; }
    public long AccountId { get; set; }
    public double Amount { get; set; }
}