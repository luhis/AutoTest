using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTest.Persistence.Setup;

public class TestRunConfig : IEntityTypeConfiguration<TestRun>
{
    public void Configure(EntityTypeBuilder<TestRun> builder)
    {
        builder.HasDiscriminatorInJsonId();
        builder.HasKey(e => e.TestRunId);
        builder.Property(e => e.TestRunId).ValueGeneratedNever().IsRequired();
        builder.Property(e => e.TimeInMS).IsRequired();
        builder.Property(e => e.Created).IsRequired();
        builder.Property(e => e.Ordinal).IsRequired();
        builder.HasOne<Event>().WithMany().HasForeignKey(p => p.EventId);
        builder.HasOne<Entrant>().WithMany().HasForeignKey(p => p.EntrantId);
        builder.HasOne<Marshal>().WithMany().HasForeignKey(p => p.MarshalId);
        builder.OwnsMany(a => a.Penalties, SetupPenalty);
    }

    public static void SetupPenalty<T>(OwnedNavigationBuilder<T, Penalty> entity) where T : class
    {
        entity.Property(e => e.InstanceCount).IsRequired();
        entity.Property(e => e.PenaltyType).IsRequired();
    }
}
