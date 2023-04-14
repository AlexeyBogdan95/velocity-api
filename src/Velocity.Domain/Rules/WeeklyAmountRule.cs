

using Microsoft.Extensions.Logging;

namespace Velocity.Domain.Rules;

public class WeeklyAmountRule: Rule
{
    public WeeklyAmountRule(ILogger<Rule> logger) : base(logger)
    {
    }

    protected override string Name => nameof(WeeklyAmountRule);

    protected override decimal ThresholdValue => 2_000_000;

    public override DateTime GetDateThreshold(DateTime dateTime)
    {
        return dateTime.FirstDateOfWeek();
    }

    protected override decimal GetThresholdValueIncrementor(Transaction transaction)
    {
        return transaction.Amount;
    }
}