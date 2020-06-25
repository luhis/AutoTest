using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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
            entity.Property(e => e.Created).HasDefaultValueSql("(getdate())").Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            entity.HasOne<Test>().WithMany().HasForeignKey(p => p.TestId);
            entity.HasOne<Entrant>().WithMany().HasForeignKey(p => p.EntrantId);
        }
    }
}
