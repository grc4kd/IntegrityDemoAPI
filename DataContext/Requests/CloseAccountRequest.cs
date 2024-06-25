namespace DataContext.Requests;

public record struct CloseAccountRequest(int CustomerId, int AccountId);