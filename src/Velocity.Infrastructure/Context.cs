using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Velocity.Domain;

namespace Velocity.Infrastructure;

public class Context: DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
    }
    
    public DbSet<Threshold> Thresholds { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveConversion(typeof(DateTimeUtcSpecifiedConverter));
        base.ConfigureConventions(configurationBuilder);
    }
}

public class DateTimeUtcSpecifiedConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeUtcSpecifiedConverter() : 
        base(c => DateTime.SpecifyKind(c, DateTimeKind.Utc),
            c => c)
    {
    }
}