using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTest.Persistence.Setup
{
    public static class SetupProfile
    {
        public static void Setup(EntityTypeBuilder<Profile> entity)
        {
            entity.HasKey(e => e.EmailAddress);
            entity.Property(e => e.EmailAddress).ValueGeneratedNever().IsRequired();
            entity.Property(e => e.GivenName).IsRequired();
            entity.Property(e => e.FamilyName).IsRequired();
            entity.OwnsOne(a => a.Vehicle, SetupVehicle.Setup);
            entity.OwnsOne(a => a.EmergencyContact, SetupEmergencyContact.Setup);
            entity.OwnsMany(a => a.ClubMemberships, SetupClubMembership.Setup);
            entity.OwnsOne(a => a.MsaMembership, SetupMsaMembership.Setup);
        }
    }
}
