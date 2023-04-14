using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velocity.Domain;

namespace Velocity.Infrastructure.Configurations;

public class ThresholdConfiguration: IEntityTypeConfiguration<Threshold>
{
    public void Configure(EntityTypeBuilder<Threshold> builder)
    {
        builder.HasKey(x => new {x.CustomerId, x.Date, x.Rule, x.Version });
        builder.HasOne(x => x.Transaction)
            .WithMany()
            .HasForeignKey(x => x.TransactionId);
    }
}