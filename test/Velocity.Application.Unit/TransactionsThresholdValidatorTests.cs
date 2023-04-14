using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Velocity.Domain;
using Velocity.Domain.Rules;
using Xunit;

namespace Velocity.Application.Unit;

public class TransactionsThresholdValidatorTests
{
    [Fact]
    public async Task Validate_TransactionsExist_ReturnsAlreadyExists()
    {
        //Arrange
        var uow = Substitute.For<IUnitOfWork>();
        uow.Transactions.Get(Arg.Any<int>(), Arg.Any<int>()).Returns(new Domain.Transaction());
        var logger = Substitute.For<ILogger<TransactionsThresholdValidator>>();
        var factory = Substitute.For<IRuleFactory>();
        var validator = new TransactionsThresholdValidator(uow, factory, logger);
        var transaction = new Transaction
        {
            Id = 1,
            CustomerId = 2,
            Amount = 400000,
            Time = DateTime.UtcNow
        };
        
        //Act
        var response = await validator.Validate(transaction, CancellationToken.None);
        
        //Assert
        factory.Received(0).Validate(Arg.Any<Domain.Transaction>(), Arg.Any<Threshold[]>());
        await uow.Received(0).Commit(Arg.Any<CancellationToken>());
        response.TransactionOutput.ShouldBeNull();
        response.AlreadyExists.ShouldBeTrue();
    }
    
    [Fact]
    public async Task Validate_RulesFactoryReturnsInvalid_ReturnsNonAcceptedTransaction()
    {
        //Arrange
        var uow = Substitute.For<IUnitOfWork>();
        uow.Transactions
            .Get(Arg.Any<int>(), Arg.Any<int>())
            .Returns(Task.FromResult<Domain.Transaction>(null));

        var logger = Substitute.For<ILogger<TransactionsThresholdValidator>>();
        var factory = Substitute.For<IRuleFactory>();
        factory
            .Validate(Arg.Any<Domain.Transaction>(),  Arg.Any<Threshold[]>())
            .Returns(new RuleValidationResult {IsValid = false, Thresholds = new Threshold[] { }});
        
        var validator = new TransactionsThresholdValidator(uow, factory, logger);
        var transaction = new Transaction
        {
            Id = 1,
            CustomerId = 2,
            Amount = 400000,
            Time = DateTime.UtcNow
        };
        
        //Act
        var response = await validator.Validate(transaction, CancellationToken.None);
        
        //Assert
        factory.Received(1).Validate(Arg.Any<Domain.Transaction>(), Arg.Any<Threshold[]>());
        await uow.Received(0).Commit(Arg.Any<CancellationToken>());
        response.TransactionOutput.Id.ShouldBe(transaction.Id);
        response.TransactionOutput.CustomerId.ShouldBe(transaction.CustomerId);
        response.TransactionOutput.Accepted.ShouldBeFalse();
        response.AlreadyExists.ShouldBeFalse();
    }
    
    [Fact]
    public async Task Validate_RulesFactoryReturnsThresholds_ReturnsAcceptedTransaction()
    {
        //Arrange
        var uow = Substitute.For<IUnitOfWork>();
        uow.Transactions
            .Get(Arg.Any<int>(), Arg.Any<int>())
            .Returns(Task.FromResult<Domain.Transaction>(null));

        var logger = Substitute.For<ILogger<TransactionsThresholdValidator>>();
        var factory = Substitute.For<IRuleFactory>();
        factory
            .Validate(Arg.Any<Domain.Transaction>(), Arg.Any<Threshold[]>())
            .Returns(new RuleValidationResult {IsValid = true, Thresholds = new Threshold[] { new() }});
        
        var validator = new TransactionsThresholdValidator(uow, factory, logger);
        var transaction = new Transaction
        {
            Id = 1,
            CustomerId = 2,
            Amount = 400000,
            Time = DateTime.UtcNow
        };
        
        //Act
        var response = await validator.Validate(transaction, CancellationToken.None);
        
        //Assert
        factory.Received(1).Validate(Arg.Any<Domain.Transaction>(), Arg.Any<Threshold[]>());
        await uow.Received(1).Commit(Arg.Any<CancellationToken>());
        response.TransactionOutput.Id.ShouldBe(transaction.Id);
        response.TransactionOutput.CustomerId.ShouldBe(transaction.CustomerId);
        response.TransactionOutput.Accepted.ShouldBeTrue();
        response.AlreadyExists.ShouldBeFalse();
    }
}