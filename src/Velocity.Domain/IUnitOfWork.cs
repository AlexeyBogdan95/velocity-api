namespace Velocity.Domain;

public interface IUnitOfWork
{
    IThresholdRepository Thresholds { get; }
    ITransactionRepository Transactions { get; }
    Task Commit(CancellationToken cancellationToken);
}