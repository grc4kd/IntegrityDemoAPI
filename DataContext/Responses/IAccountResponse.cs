namespace DataContext.Responses;

public interface IAccountResponse
{
    public int CustomerId { get; }
    public int AccountId { get; }
    public bool Succeeded { get; }
}