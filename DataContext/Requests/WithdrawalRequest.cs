namespace DataContext.Requests;

public record struct WithdrawalRequest(long CustomerId, long AccountId, double Amount);