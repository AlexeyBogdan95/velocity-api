namespace Velocity.Domain;

public interface IThresholdRepository
{
    Task<IReadOnlyCollection<Threshold>> Get(int customerId, DateTime[] dateTime, CancellationToken cancellationToken);
    void AddMany(IEnumerable<Threshold> rule, CancellationToken cancellationToken);
}