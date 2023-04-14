using Microsoft.Extensions.DependencyInjection;
using Velocity.Domain.Rules;

namespace Velocity.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddTransient<Rule, DailyAmountRule>();
        services.AddTransient<Rule, WeeklyAmountRule>();
        services.AddTransient<Rule, DailyCountRule>();
        services.AddTransient<IRuleFactory, RuleFactory>();
        return services;
    }
}

