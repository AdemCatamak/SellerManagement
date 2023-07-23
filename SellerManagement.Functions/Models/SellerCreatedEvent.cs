namespace SellerManagement.Functions.Models;

public record SellerCreatedEvent
{
    public string Email { get; init; } = string.Empty;
}