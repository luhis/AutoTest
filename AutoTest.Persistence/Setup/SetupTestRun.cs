namespace AutoTest.Persistence.Setup
{
    using AutoTest.Domain.StorageModels;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SetupTestRun
    {
        public static void Setup(EntityTypeBuilder<TestRun> entity)
        {
            entity.HasKey(e => e.TestRunId);
            entity.Property(e => e.TestRunId).ValueGeneratedNever().IsRequired();
            entity.Property(e => e.TimeInMS).IsRequired();
            entity.Property(e => e.Created).IsRequired();
            entity.Property(e => e.Ordinal).IsRequired();
            entity.HasOne<Event>().WithMany().HasForeignKey(p => p.EventId);
            entity.HasOne<Entrant>().WithMany().HasForeignKey(p => p.EntrantId);
            entity.OwnsMany(a => a.Penalties, SetupPenalty);
        }
        public static void SetupPenalty(OwnedNavigationBuilder<TestRun, Penalty> entity)
        {
            entity.Property(e => e.InstanceCount).IsRequired();
            entity.Property(e => e.PenaltyType).IsRequired();
        }
    }
}
