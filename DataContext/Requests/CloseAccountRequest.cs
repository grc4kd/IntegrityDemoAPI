namespace DataContext.Requests;

public class CloseAccountRequest
{
    public long CustomerId { get; set; }
    public long AccountId { get; set; }
}