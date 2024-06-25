namespace DataContext.Requests;

public record struct DepositRequest(int CustomerId, int AccountId, double Amount);