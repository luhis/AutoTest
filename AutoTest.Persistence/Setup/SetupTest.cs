namespace AutoTest.Persistence.Setup
{
    using AutoTest.Domain.StorageModels;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SetupTest
    {
        public static void Setup(EntityTypeBuilder<Test> entity)
        {
            entity.HasKey(e => e.TestId);
            entity.Property(e => e.TestId).ValueGeneratedNever().IsRequired();
            entity.Property(e => e.Ordinal).IsRequired();
            entity.Property(e => e.MapLocation);
            entity.HasOne<Event>().WithMany().HasForeignKey(p => p.EventId);
        }
    }
}