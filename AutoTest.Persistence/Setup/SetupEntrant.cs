namespace AutoTest.Persistence.Setup
{
    using AutoTest.Domain.StorageModels;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SetupEntrant
    {
        public static void Setup(EntityTypeBuilder<Entrant> entity)
        {
            entity.HasKey(e => e.EntrantId);
            entity.Property(e => e.EntrantId).ValueGeneratedNever().IsRequired();
            entity.Property(e => e.Class).IsRequired();
            entity.Property(e => e.GivenName).IsRequired();
            entity.Property(e => e.FamilyName).IsRequired();
            entity.Property(e => e.IsPaid).IsRequired();
            entity.HasOne<Event>().WithMany().HasForeignKey(p => p.EventId);
            entity.OwnsOne(a => a.Vehicle, SetupVehicle.Setup);
        }
    }
}
