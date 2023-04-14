namespace Velocity.Domain;

public class Threshold
{
    public string Rule { get; set; }
    public decimal Amount { get; set; }
    public int CustomerId { get; set; }
    public DateTime Date { get; set; }
    public int Version { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public Guid TransactionId { get; set; }
    public Transaction Transaction { get; set; }

    public static Threshold Create(int customerId, string rule, decimal amount, Guid transactionId, DateTime dateTime)
    {
        return new Threshold
        {
            CustomerId = customerId,
            Rule = rule,
            Amount = amount,
            Date = dateTime,
            Version = 1,
            TransactionId = transactionId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
    
    public Threshold CreateNewVersion(decimal amount, Guid transactionId)
    {
        return new Threshold
        {
            CustomerId = CustomerId,
            Rule = Rule,
            Amount = amount,
            Date = Date,
            Version = Version + 1,
            TransactionId = transactionId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}