namespace DataContext.Responses;

public record AccountResponse(long CustomerId, long AccountId, bool Succeeded);