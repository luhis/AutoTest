namespace AutoTest.Persistence.Setup
{
    using AutoTest.Domain.StorageModels;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SetupTest
    {
        public static void Setup(EntityTypeBuilder<Test> entity)
        {
            entity.HasKey(e => e.TestId);
            entity.HasOne<Event>().WithMany().HasForeignKey(p => p.EventId);
        }
    }
}