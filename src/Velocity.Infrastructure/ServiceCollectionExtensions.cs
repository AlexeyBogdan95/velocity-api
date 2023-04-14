using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Velocity.Domain;

namespace Velocity.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<Context>(builder => builder.UseNpgsql(connectionString));
        services.AddScoped(provider =>
        {
            var context = provider.GetService<Context>();
            return context!.GetService<IMigrator>();
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}

