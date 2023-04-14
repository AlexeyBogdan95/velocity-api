using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Velocity.Domain;

namespace Velocity.Infrastructure;

public class TransactionRepository: ITransactionRepository
{
    private readonly DbSet<Transaction> _transactions;

    public TransactionRepository(DbSet<Transaction> transactions)
    {
        _transactions = transactions;
    }

    public Task<Transaction> Get(int providerId, int customerId)
    {
        return _transactions
            .SingleOrDefaultAsync(x => x.ProviderId == providerId && x.CustomerId == customerId);
    }

    public void Add(Transaction transaction, CancellationToken cancellationToken)
    {
        _transactions.AddAsync(transaction, cancellationToken);
    }
}