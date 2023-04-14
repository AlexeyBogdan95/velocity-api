using Microsoft.Extensions.Logging;

namespace Velocity.Domain.Rules;

public abstract class Rule
{
    private readonly ILogger<Rule> _logger;

    protected Rule(ILogger<Rule> logger)
    {
        _logger = logger;
    }

    protected abstract decimal ThresholdValue { get; }
    protected abstract string Name { get; }
    public abstract DateTime GetDateThreshold(DateTime dateTime);
    protected abstract decimal GetThresholdValueIncrementor(Transaction transaction);
    
    public Threshold Validate(Transaction transaction, Threshold[] thresholds)
    {
        var thresholdDate = GetDateThreshold(transaction.Time);
        var threshold = thresholds.FirstOrDefault(x => x.Rule == Name && x.Date == thresholdDate);

        decimal currentAmount = threshold?.Amount ?? 0;
        decimal newAmount = currentAmount + GetThresholdValueIncrementor(transaction);
        if (newAmount > ThresholdValue)
        {
            _logger.LogWarning(
                "Failed to validate rule {rule_name} for transaction {transaction_id}. Current value: {current_value}. Threshold value: {threshold_value}",
                Name, transaction.Id, newAmount, ThresholdValue);
            return null;
        }


        if (threshold == null)
        {
            return Threshold.Create(transaction.CustomerId, Name, newAmount, transaction.Id, thresholdDate);
        }

        var newVersion = threshold.CreateNewVersion(newAmount, transaction.Id);
        return newVersion;
    }
}