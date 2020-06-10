namespace AutoTest.Persistence.Setup
{
    using AutoTest.Domain.StorageModels;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SetupTestRun
    {
        public static void Setup(EntityTypeBuilder<TestRun> entity)
        {
            entity.HasKey(e => e.TestRunId);
            entity.HasOne<Test>().WithMany().HasForeignKey(p => p.TestId);
            entity.HasOne<Entrant>().WithMany().HasForeignKey(p => p.Entrant);
        }
    }
}