namespace Velocity.Domain.Rules;

public class RuleValidationResult
{
    public required Threshold[] Thresholds { get; set; }
    public required bool IsValid { get; set; }

    public static RuleValidationResult Success(Threshold[] thresholds)
    {
        return new RuleValidationResult
        {
            Thresholds = thresholds,
            IsValid = true
        };
    }
    
    public static RuleValidationResult Failure()
    {
        return new RuleValidationResult
        {
            Thresholds = Array.Empty<Threshold>(),
            IsValid = false
        };
    }
}