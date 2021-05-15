using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTest.Persistence.Setup
{
    public static class SetupEmergencyContact
    {
        public static void Setup<T>(OwnedNavigationBuilder<T, EmergencyContact> entity) where T : class
        {
            entity.WithOwner();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Phone).IsRequired();
        }
    }
    public static class SetupClubMembership
    {
        public static void Setup<T>(OwnedNavigationBuilder<T, ClubMembership> entity) where T : class
        {
            entity.WithOwner();
            entity.Property(e => e.ClubName).IsRequired();
            entity.Property(e => e.MembershipNumber).IsRequired();
            entity.Property(e => e.Expiry).IsRequired();
        }
    }
    public static class SetupMsaMembership
    {
        public static void Setup<T>(OwnedNavigationBuilder<T, MsaMembership> entity) where T : class
        {
            entity.WithOwner();
            entity.Property(e => e.MsaLicenseType).IsRequired();
            entity.Property(e => e.MsaLicense).IsRequired();
        }
    }
}
