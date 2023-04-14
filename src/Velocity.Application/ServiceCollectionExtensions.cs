using Microsoft.Extensions.DependencyInjection;

namespace Velocity.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<ITransactionsThresholdValidator, TransactionsThresholdValidator>();
        return services;
    }
}

