using Microsoft.Extensions.Logging;
using Velocity.Domain;
using Velocity.Domain.Rules;

namespace Velocity.Application;

public class TransactionsThresholdValidator: ITransactionsThresholdValidator
{
    private readonly IUnitOfWork _uow;
    private readonly IRuleFactory _ruleFactory;
    private readonly ILogger<TransactionsThresholdValidator> _logger;

    public TransactionsThresholdValidator(IUnitOfWork uow, IRuleFactory ruleFactory, ILogger<TransactionsThresholdValidator> logger)
    {
        _uow = uow;
        _ruleFactory = ruleFactory;
        _logger = logger;
    }

    public async Task<ValidateTransactionResponse> Validate(Transaction transaction, CancellationToken cancellationToken)
    {
        try
        {
            var domainTransaction = transaction.ToDomain();
            var existedTransaction = await _uow.Transactions.Get(
                domainTransaction.ProviderId, domainTransaction.CustomerId);

            if (existedTransaction != null)
            {
                _logger.LogWarning(
                    "Transaction {transaction_id} of customer {customer_id} already exists",
                    transaction.Id, transaction.CustomerId);
                return ValidateTransactionResponse.MarkAsAlreadyExists();
            }

            var thresholdDates = _ruleFactory.GetThresholdDates(domainTransaction);
            var thresholds = await _uow.Thresholds.Get(transaction.CustomerId, thresholdDates, cancellationToken);

            var result = _ruleFactory.Validate(domainTransaction, thresholds.ToArray());
            if (result.IsValid)
            {
                _uow.Thresholds.AddMany(result.Thresholds, cancellationToken);
                _uow.Transactions.Add(domainTransaction, cancellationToken);
                await _uow.Commit(cancellationToken);
            }

            return ValidateTransactionResponse.Ok(new TransactionOutput
            {
                Id = transaction.Id,
                CustomerId = transaction.CustomerId,
                Accepted = result.IsValid
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to validate transaction {transaction_id}", transaction.Id);
            throw;
        }
    }
}