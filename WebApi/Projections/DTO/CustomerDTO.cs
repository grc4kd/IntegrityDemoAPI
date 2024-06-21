namespace WebApi.Projections.DTO;

public record Customer
{
    public long Id { get; set; }
    public required string Name { get; set; }
}