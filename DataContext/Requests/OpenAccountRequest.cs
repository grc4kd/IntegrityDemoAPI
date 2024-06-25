namespace DataContext.Requests;

public record struct OpenAccountRequest(long CustomerId, double InitialDeposit, int AccountTypeId);
