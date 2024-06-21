namespace DataContext.Requests;

public record DepositRequest
{
    public long customerId;
    public long accountId;
    public double amount;
}