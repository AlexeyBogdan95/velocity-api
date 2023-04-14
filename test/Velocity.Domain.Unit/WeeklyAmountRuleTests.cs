using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Velocity.Domain.Rules;

namespace Velocity.Domain.Unit;

public class WeeklyAmountRuleTests
{
    [Fact]
    public async Task Validate_ThresholdExistsAndExceedsThreshold_ReturnNull()
    {
        //Arrange
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(), 
            Amount = 15_000_000, 
            CustomerId = 123, 
            Time = DateTime.UtcNow
        };
        
        var threshold = new Threshold
        {
            Rule = nameof(WeeklyAmountRule),
            Amount = 10_000_000,
            CustomerId = 123,
            Date = DateTime.UtcNow.FirstDateOfWeek(),
            Version = 1
        };

        
        var logger = Substitute.For<ILogger<Rule>>();
        var rule = new WeeklyAmountRule(logger);
        
        //Act
        var result = rule.Validate(transaction, new[] {threshold});
        
        //Assert
        result.ShouldBeNull();
    }
    
    [Fact]
    public async Task Validate_ThresholdDoesNotExistAndExceedsThreshold_ReturnNull()
    {
        //Arrange
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(), 
            Amount = 30_000_000, 
            CustomerId = 123, 
            Time = DateTime.UtcNow
        };
        
        var fakeThreshold = new Threshold
        {
            Rule = nameof(WeeklyAmountRule),
            Amount = 500000,
            CustomerId = 456,
            Date = DateTime.UtcNow.FirstDateOfWeek(),
            Version = 1
        };

        var logger = Substitute.For<ILogger<Rule>>();
        var rule = new WeeklyAmountRule(logger);
        
        //Act
        var result = rule.Validate(transaction, new[] { fakeThreshold });
        
        //Assert
        result.ShouldBeNull();
    }
    
    [Fact]
    public async Task Validate_ThresholdDoesNotExist_CreatesNewThreshold()
    {
        //Arrange
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(), 
            Amount = 500000, 
            CustomerId = 123, 
            Time = DateTime.UtcNow
        };

        var logger = Substitute.For<ILogger<Rule>>();
        var rule = new WeeklyAmountRule(logger);
        
        //Act
        var result = rule.Validate(transaction, new Threshold[] {});
        
        //Assert
        result.ShouldNotBeNull();
        result.Version.ShouldBe(1);
        result.Amount.ShouldBe(transaction.Amount);
        result.CustomerId.ShouldBe(transaction.CustomerId);
        result.TransactionId.ShouldBe(transaction.Id);
    }

    [Fact]
    public async Task Validate_ThresholdAlreadyExists_CreatesNewThresholdVersion()
    {
        //Arrange
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(), 
            Amount = 500000, 
            CustomerId = 123, 
            Time = DateTime.UtcNow
        };

        var threshold = new Threshold
        {
            Rule = nameof(WeeklyAmountRule),
            Amount = 500000,
            CustomerId = 123,
            Date = DateTime.UtcNow.FirstDateOfWeek(),
            Version = 1
        };

        
        var logger = Substitute.For<ILogger<Rule>>();
        var rule = new WeeklyAmountRule(logger);
        
        //Act
        var result = rule.Validate(transaction, new[] { threshold });
        
        //Assert
        result.ShouldNotBeNull();
        result.Version.ShouldBe(2);
        result.Amount.ShouldBe(transaction.Amount + threshold.Amount);
        result.CustomerId.ShouldBe(transaction.CustomerId);
        result.TransactionId.ShouldBe(transaction.Id);
    }
}