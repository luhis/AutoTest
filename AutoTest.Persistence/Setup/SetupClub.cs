namespace AutoTest.Persistence.Setup
{
    using AutoTest.Domain.StorageModels;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SetupClub
    {
        public static void Setup(EntityTypeBuilder<Club> entity)
        {
            entity.HasKey(e => e.ClubId);
            entity.Property(e => e.ClubId).ValueGeneratedNever().IsRequired();
            entity.Property(e => e.ClubName).IsRequired();
            entity.Property(e => e.ClubPaymentAddress).IsRequired();
            entity.Property(e => e.Website).IsRequired();
            entity.OwnsMany(a => a.AdminEmails, SetupAuthorisationEmails.Setup);
        }
    }
}
