namespace DataContext.Requests;

public record CloseAccountRequest
{
    public long CustomerId { get; set; }
    public long AccountId { get; set; }
}