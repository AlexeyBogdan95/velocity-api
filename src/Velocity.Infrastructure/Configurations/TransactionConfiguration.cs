using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velocity.Domain;

namespace Velocity.Infrastructure.Configurations;

public class TransactionConfiguration: IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new {x.ProviderId, x.CustomerId}).IsUnique();
    }
}