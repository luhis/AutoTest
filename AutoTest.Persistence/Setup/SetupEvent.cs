namespace AutoTest.Persistence.Setup
{
    using AutoTest.Domain.StorageModels;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SetupEvent
    {
        public static void Setup(EntityTypeBuilder<Event> entity)
        {
            entity.HasKey(e => e.EventId);
            entity.Property(e => e.EventId).ValueGeneratedNever().IsRequired();
            entity.Property(e => e.Location).IsRequired();
            entity.Property(e => e.StartTime).IsRequired();
            entity.HasOne<Club>().WithMany().HasForeignKey(p => p.ClubId);
        }
    }
}