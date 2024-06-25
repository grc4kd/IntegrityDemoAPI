namespace DataContext.Requests;

public record struct WithdrawalRequest(int CustomerId, int AccountId, double Amount);