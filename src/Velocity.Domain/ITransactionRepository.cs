namespace Velocity.Domain;

public interface ITransactionRepository
{
    Task<Transaction> Get(int providerId, int customerId);
    void Add(Transaction transaction, CancellationToken cancellationToken);
}