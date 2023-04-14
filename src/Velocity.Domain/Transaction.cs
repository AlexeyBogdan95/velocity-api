namespace Velocity.Domain;

public class Transaction
{
    public Guid Id { get; set; }
    public int ProviderId { get; set; }
    public int CustomerId { get; set; }
    public int Amount { get; set; }
    public DateTime Time { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}