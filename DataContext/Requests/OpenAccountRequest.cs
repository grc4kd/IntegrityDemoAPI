namespace DataContext.Requests;

public record struct OpenAccountRequest(int CustomerId, double InitialDeposit, int AccountTypeId);
