using System.Threading;
using System.Threading.Tasks;
using Velocity.Domain;

namespace Velocity.Infrastructure;

public class UnitOfWork: IUnitOfWork
{
    private readonly Context _context;

    public UnitOfWork(Context context)
    {
        _context = context;
    }

    public IThresholdRepository Thresholds => new ThresholdRepository(_context.Thresholds);
    public ITransactionRepository Transactions => new TransactionRepository(_context.Transactions);

    public Task Commit(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}