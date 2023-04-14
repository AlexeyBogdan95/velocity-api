namespace Velocity.Application;

public interface ITransactionsThresholdValidator
{
    Task<ValidateTransactionResponse> Validate(Transaction transaction, CancellationToken cancellationToken);
}