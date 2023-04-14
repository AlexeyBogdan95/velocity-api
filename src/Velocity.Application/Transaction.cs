namespace Velocity.Application;

public class Transaction
{
    public required int Id { get; set; }
    public required int CustomerId { get; set; }
    public required int Amount { get; set; }
    public required DateTime Time { get; set; }

    public Domain.Transaction ToDomain()
    {
        return new Domain.Transaction
        {
            Id = Guid.NewGuid(),
            ProviderId = Id,
            CustomerId = CustomerId,
            Amount = Amount,
            Time = Time,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
