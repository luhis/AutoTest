namespace AutoTest.Persistence.Setup
{
    using AutoTest.Domain.StorageModels;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SetupEntrant
    {
        public static void Setup(EntityTypeBuilder<Entrant> entity)
        {
            entity.HasKey(e => e.EntrantId);
            entity.HasOne<Event>().WithMany().HasForeignKey(p => p.EventId);
        }
    }
}