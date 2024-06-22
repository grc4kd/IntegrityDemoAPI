namespace DataContext.Responses;

public record AccountResponse(long CustomerId, long AccountId, decimal Balance, bool Succeeded);