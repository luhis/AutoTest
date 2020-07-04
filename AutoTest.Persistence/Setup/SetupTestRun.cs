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
            entity.HasOne<Test>().WithMany().HasForeignKey(p => p.TestId);
            entity.HasOne<Entrant>().WithMany().HasForeignKey(p => p.EntrantId);
            entity.OwnsMany(a => a.Penalties, SetupPenalty);
        }
        public static void SetupPenalty(OwnedNavigationBuilder<TestRun, Penalty> entity)
        {
            entity.WithOwner().HasForeignKey(e => e.TestRunId);
            //entity.Property(e => e.PenaltyId).ValueGeneratedNever().IsRequired();
            entity.Property(e => e.InstanceCount).IsRequired();
            entity.Property(e => e.PenaltyType).IsRequired();
        }
    }
}
