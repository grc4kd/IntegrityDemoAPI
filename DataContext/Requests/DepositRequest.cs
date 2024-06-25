namespace DataContext.Requests;

public record struct DepositRequest(long CustomerId, long AccountId, double Amount);