namespace DataContext.Responses;

public interface IAccountResponse
{
    public long CustomerId { get; }
    public long AccountId { get; }
    public bool Succeeded { get; }
}