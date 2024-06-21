namespace DataContext.Requests;

public record DepositRequest
{
    public long CustomerId { get; set; }
    public long AccountId { get; set; }
    public double Amount { get; set; }
}