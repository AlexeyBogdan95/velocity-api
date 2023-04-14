using Microsoft.Extensions.Logging;

namespace Velocity.Domain.Rules;

public class DailyAmountRule: Rule
{
    public DailyAmountRule(ILogger<Rule> logger) : base(logger)
    {
    }
    
    protected override string Name => nameof(DailyAmountRule);
    protected override decimal ThresholdValue => 500000;

    public override DateTime GetDateThreshold(DateTime dateTime)
    {
        return dateTime.Date;
    }

    protected override decimal GetThresholdValueIncrementor(Transaction transaction)
    {
        return transaction.Amount;
    }
    

}