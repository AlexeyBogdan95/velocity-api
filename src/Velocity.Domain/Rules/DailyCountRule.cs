using Microsoft.Extensions.Logging;

namespace Velocity.Domain.Rules;

public class DailyCountRule: Rule
{
    public DailyCountRule(ILogger<Rule> logger) : base(logger)
    {
    }
    
    protected override string Name => nameof(DailyCountRule);

    protected override decimal ThresholdValue => 3;

    public override DateTime GetDateThreshold(DateTime dateTime)
    {
        return dateTime.Date;
    }

    protected override decimal GetThresholdValueIncrementor(Transaction transaction)
    {
        return 1;
    }
}