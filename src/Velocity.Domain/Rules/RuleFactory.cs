namespace Velocity.Domain.Rules;

public interface IRuleFactory
{
    RuleValidationResult Validate(Transaction transaction, Threshold[] thresholds);
    DateTime[] GetThresholdDates(Transaction transaction);
}

public class RuleFactory: IRuleFactory
{
    private readonly IEnumerable<Rule> _rules;

    public RuleFactory(IEnumerable<Rule> rules)
    {
        _rules = rules;
    }


    public RuleValidationResult Validate(Transaction transaction, Threshold[] thresholds)
    {
        List<Threshold> newThresholds = new List<Threshold>();

        foreach (var rule in _rules)
        {
            var newThreshold = rule.Validate(transaction, thresholds);
            if (newThreshold == null)
            {
                return RuleValidationResult.Failure();
            }
            
            newThresholds.Add(newThreshold);
        }
        
        return RuleValidationResult.Success(newThresholds.ToArray());
    }

    public DateTime[] GetThresholdDates(Transaction transaction)
    {
        return _rules.Select(x => x.GetDateThreshold(transaction.Time)).ToArray();
    }
}
