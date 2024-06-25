namespace DataContext.Requests;

public record struct CloseAccountRequest(long CustomerId, long AccountId);